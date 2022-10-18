using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Server.Helpers.Interfaces;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.SharedBackend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PeopleController : ControllerBase
    {
  
        private readonly IPersonRepository _personRepository;
       

        public PeopleController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }



        [HttpPost("create")]
        public async Task<ActionResult<int>> Post(Person person)
        {
            await _personRepository.CreatePerson(person);
            return person.Id;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get([FromQuery] PaginationDto paginationDto)
        {

            var paginatedReponse = await _personRepository.GetPeople(paginationDto);
            HttpContext.InsertPaginationParametersInResponse(paginatedReponse.TotalAmountPages);


            return paginatedReponse.Response;
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            return await _personRepository.GetPeopleByName(searchText);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _personRepository.GetPersonById(id);
            if (person == null)
                return NotFound();
            return person;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Person person)
        {
            var personDb = await _personRepository.GetPersonById(person.Id);

            if (personDb == null)
                return NotFound();
            await _personRepository.UpdatePerson(person);   
 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _personRepository.GetPersonById(id);
            if (person == null)
                return NotFound();

            await _personRepository.DeletePerson(id);
            return NoContent();
        }


    }
}
