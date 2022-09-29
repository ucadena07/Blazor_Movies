using BlazorMovies.Server.Helpers.Interfaces;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileAzureService _fileAzureService;
        public PeopleController(ApplicationDbContext context, IFileAzureService fileAzureService)
        {
            _context = context;
            _fileAzureService = fileAzureService;   
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            if (!string.IsNullOrEmpty(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                //person.Picture = await _fileAzureService.SaveFile(personPicture, ".jpg", "people");
            }

            _context.Add(person);
            await _context.SaveChangesAsync();
            return person.Id;
        }


    }
}
