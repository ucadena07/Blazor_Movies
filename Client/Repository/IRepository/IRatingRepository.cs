using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository.IRepository
{
    public interface IRatingRepository
    {
        Task Vote(MovieRating movieRating);
    }
}
