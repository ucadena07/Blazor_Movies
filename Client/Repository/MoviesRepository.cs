using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;
using System.Text.RegularExpressions;

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
            var response = await _httpService.Post<Movie,int>(url, movie, includeToken: true);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<IndexPageDTO> GetIndexPageDto()
        {
            return await _httpService.GetHelper<IndexPageDTO>(url, includeToken: false);
        }

        public async Task<DetailsMovieDTO> GetDetailsDTO(int id)
        {
            return await _httpService.GetHelper<DetailsMovieDTO>($"{url}/{id}", includeToken: false);
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

        public async Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMovieDto filterMovieDto)
        {
            var httpResp = await _httpService.Post<FilterMovieDto,List<Movie>>($"{url}/filter",filterMovieDto, includeToken: false);
            var totalAmountPages = int.Parse(httpResp.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());
            var paginatedResp = new PaginatedResponse<List<Movie>>()
            {
                Response = httpResp.Response,
                TotalAmountPages = totalAmountPages
            };
            return paginatedResp;
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
