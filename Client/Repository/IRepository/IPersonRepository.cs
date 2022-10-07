using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository.IRepository
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
        Task DeletePerson(int Id);
        Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDto paginationDto);
        Task<List<Person>> GetPeopleByName(string name);
        Task<Person> GetPersonById(int id);
        Task UpdatePerson(Person person);
    }
}
