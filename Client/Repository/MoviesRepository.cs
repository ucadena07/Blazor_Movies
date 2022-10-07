using BlazorMovies.Client.Helpers;
using BlazorMovies.Client.Pages.Movies;
using BlazorMovies.Client.Repository.IRepository;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository
{
    public class MoviesRepository : IMovieRepository
    {
        private readonly IHttpService _httpService;
        private string url = "api/movies";
        public MoviesRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<int> CreateMovie(Movie movie)
        {
            var response = await _httpService.Post<Movie,int>(url, movie);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<IndexPageDTO> GetIndexPageDto()
        {
            return await _httpService.GetHelper<IndexPageDTO>(url);
        }

        public async Task<DetailsMovieDTO> GetDetailsDTO(int id)
        {
            return await _httpService.GetHelper<DetailsMovieDTO>($"{url}/{id}");
        }

        public async Task<MovieUpdateDto> GetMovieForUpdate(int id)
        {
            return await _httpService.GetHelper<MovieUpdateDto>($"{url}/update/{id}");
        }

        public async Task DeleteMovie(int Id)
        {
            var response = await _httpService.Delete<Movie>($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }


        public async Task UpdateMovie(Movie movie)
        {
            var response = await _httpService.Put(url, movie);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
        //private async Task<T> Get<T>(string url)
        //{
        //    var response = await _httpService.Get<T>(url);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }
        //    return response.Response;
        //}
    }
}
