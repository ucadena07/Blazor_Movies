using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository.IRepository
{
    public interface IMovieRepository
    {
        Task<int> CreateMovie(Movie movie);
        Task<DetailsMovieDTO> GetDetailsDTO(int id);
        Task<IndexPageDTO> GetIndexPageDto();
    }
}
