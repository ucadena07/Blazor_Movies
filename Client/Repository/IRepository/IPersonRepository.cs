using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository.IRepository
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
        Task DeletePerson(int Id);
        Task<List<Person>> GetPeople();
        Task<List<Person>> GetPeopleByName(string name);
        Task<Person> GetPersonById(int id);
        Task UpdatePerson(Person person);
    }
}
