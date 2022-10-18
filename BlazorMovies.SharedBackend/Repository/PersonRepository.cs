using AutoMapper;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.SharedBackend.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.SharedBackend.Repository
{


    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticationStateService _authStateService;
        private readonly IFileService _fileService;
        private IMapper _mapper;

        public PersonRepository(ApplicationDbContext context, IAuthenticationStateService stateService, IFileService fileService, IMapper mapper)
        {
            _context = context;
            _authStateService = stateService;
            _fileService = fileService;
            _mapper = mapper;
        }
        public Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDto paginationDto)
        {
            var quaryable = _context.People.AsQueryable();
            return quaryable.GetPaginatedResponse(paginationDto);
        }

        public async Task<List<Person>> GetPeopleByName(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return new List<Person>();
            return await _context.People.Where(it => it.Name.Contains(searchText)).Take(5).ToListAsync();
        }

        public async Task<Person> GetPersonById(int id)
        {
           return await _context.People.FindAsync(id);
        }

        public async Task CreatePerson(Person person)
        {
            if (!string.IsNullOrEmpty(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                //person.Picture = await _fileAzureService.SaveFile(personPicture, ".jpg", "people");
                person.Picture = await _fileService.SaveFile(personPicture, ".jpg", "people");
            }

            _context.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePerson(int Id)
        {
            var person = await GetPersonById(Id);   
            _context.Remove(person);
            await _context.SaveChangesAsync();
        }


        public async Task UpdatePerson(Person person)
        {
            _context.Entry(person).State = EntityState.Detached;
            var personDb = await GetPersonById(person.Id);
            personDb = _mapper.Map(person, personDb);

            if (!string.IsNullOrEmpty(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                personDb.Picture = await _fileService.EditFile(personPicture, "jpg", "people", personDb.Picture);
            }

            await _context.SaveChangesAsync();
        }
    }
}
