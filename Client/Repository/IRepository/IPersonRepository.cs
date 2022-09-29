using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository.IRepository
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
    }
}
