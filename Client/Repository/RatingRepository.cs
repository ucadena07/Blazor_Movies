using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IHttpService _httpService;
        private string url = "api/rating";
        public RatingRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task Vote(MovieRating movieRating)
        {
            var httpResponse = await _httpService.Post(url, movieRating);

            if (!httpResponse.Success)
            {
                throw new ApplicationException(await httpResponse.GetBody());
            }
        }
    }
}
