using BlazorMovies.Client.Services.IService;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Services
{
    public class Repository : IRepository
    {
        public List<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie() { Title = "Spider-Man: Far From Home", ReleaseDate = new DateTime(2019, 7, 2), Poster= "https://www.sonypictures.com/sites/default/files/styles/max_560x840/public/title-key-art/spidermannowayhome_onesheet_rating.jpg?itok=tjZOthvB"},
                new Movie() { Title = "Moana", ReleaseDate = new DateTime(2016, 11, 23), Poster = "https://lumiere-a.akamaihd.net/v1/images/p_moana_20530_214883e3.jpeg"},
                new Movie() { Title = "Inception", ReleaseDate = new DateTime(2010, 7, 16), Poster = "https://m.media-amazon.com/images/M/MV5BMTM0MjUzNjkwMl5BMl5BanBnXkFtZTcwNjY0OTk1Mw@@._V1_.jpg"}
            };
        }
    }
}
