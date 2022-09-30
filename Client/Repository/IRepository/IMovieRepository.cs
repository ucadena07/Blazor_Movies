using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository.IRepository
{
    public interface IMovieRepository
    {
        Task<int> CreateMovie(Movie movie);
    }
}
