using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Server.Helpers.Interfaces;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileAzureService _fileAzureService;
        private readonly IFileService _fileService;
        private IMapper _mapper;

        public PeopleController(ApplicationDbContext context, IFileAzureService fileAzureService, IFileService fileService, IMapper mapper)
        {
            _context = context;
            _fileAzureService = fileAzureService;
            _fileService = fileService;
            _mapper = mapper;
        }

        public IFileService FileService { get; }

        [HttpPost("create")]
        public async Task<ActionResult<int>> Post(Person person)
        {
            if (!string.IsNullOrEmpty(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                //person.Picture = await _fileAzureService.SaveFile(personPicture, ".jpg", "people");
                person.Picture = await _fileService.SaveFile(personPicture, ".jpg", "people");
            }

            _context.Add(person);
            await _context.SaveChangesAsync();
            return person.Id;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get([FromQuery] PaginationDto paginationDto)
        {

            var quaryable = _context.People.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(quaryable, paginationDto.RecordsPerPage);


            return await quaryable.Paginate(paginationDto).ToListAsync();
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            if(string.IsNullOrEmpty(searchText))
                return new List<Person>();
            return await _context.People.Where(it => it.Name.Contains(searchText)).Take(5).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(it => it.Id == id);
            if (person == null)
                return NotFound();
            return person;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Person person)
        {
            var personDb = await _context.People.FirstOrDefaultAsync(it => it.Id == person.Id);

            if (personDb == null)
                return NotFound();

            personDb = _mapper.Map(person, personDb);

            if (!string.IsNullOrEmpty(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                personDb.Picture = await _fileService.EditFile(personPicture,"jpg","people",personDb.Picture);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(it => it.Id == id);
            if (person == null)
                return NotFound();

            _context.Remove(person);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
