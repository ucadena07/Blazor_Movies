using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Services.IService
{
    public interface IRepository
    {
        List<Movie> GetMovies();
    }
}
