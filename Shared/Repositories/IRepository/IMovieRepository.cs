using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Shared.Repository.IRepository
{
    public interface IMovieRepository
    {
        Task<int> CreateMovie(Movie movie);
        Task DeleteMovie(int Id);
        Task<DetailsMovieDTO> GetDetailsDTO(int id);
        Task<IndexPageDTO> GetIndexPageDto();
        Task<MovieUpdateDto> GetMovieForUpdate(int id);
        Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMovieDto filterMovieDto);
        Task UpdateMovie(Movie movie);
    }
}
