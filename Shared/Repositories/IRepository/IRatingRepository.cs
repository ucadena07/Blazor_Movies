using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Shared.Repository.IRepository
{
    public interface IRatingRepository
    {
        Task Vote(MovieRating movieRating);
    }
}
