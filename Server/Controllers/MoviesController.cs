using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Server.Helpers.Interfaces;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFileAzureService _fileAzureService;
        private readonly IFileService _fileService;
        private IMapper _mapper;
        public MoviesController(ApplicationDbContext context, IFileAzureService fileAzureService, IFileService fileService, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _fileAzureService = fileAzureService;
            _fileService = fileService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            if (!string.IsNullOrEmpty(movie.Poster))
            {
                var porster = Convert.FromBase64String(movie.Poster);
                //person.Picture = await _fileAzureService.SaveFile(personPicture, ".jpg", "people");
                movie.Poster = await _fileService.SaveFile(porster, ".jpg", "people");
            }

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            _context.Add(movie);
            await _context.SaveChangesAsync();
            return movie.Id;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            var limit = 6;

            var moviesInTheater = await _context.Movies
                .Where(it => it.InTheathers).Take(limit)
                .OrderByDescending(it => it.ReleaseDate)
                .ToListAsync();

            var todaysDate = DateTime.Now;

            var upcomingReleases = await _context.Movies
              .Where(it => it.ReleaseDate > todaysDate).Take(limit)
              .OrderByDescending(it => it.ReleaseDate)
              .ToListAsync();

            var response = new IndexPageDTO
            {
                InTheaters = moviesInTheater,
                UpcomingReleases = upcomingReleases,
            };

            return response;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DetailsMovieDTO>> Get(int id)
        {
            var movie = await _context.Movies.Where(it => it.Id == id)
                .Include(it => it.MoviesGenres).ThenInclude(it => it.Genre)
                .Include(it => it.MoviesActors).ThenInclude(it => it.Person)
                .FirstOrDefaultAsync();
            if (movie == null)
            {
                return NotFound();
            }

            var voteAvg = 0.0;
            var userVote = 0;

            if(await _context.MovieRatings.AnyAsync(it => it.MovieId == id))
            {
                voteAvg = await _context.MovieRatings.Where(it => it.MovieId == id).AverageAsync(it => it.Rate);

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
                    var userId = user.Id;

                    var userVoteDb = await _context.MovieRatings.FirstOrDefaultAsync(it => it.MovieId == id && it.UserId == userId);

                    if(userVoteDb != null)
                    {
                        userVote = userVoteDb.Rate;
                    }
                }
            }

            movie.MoviesActors = movie.MoviesActors.OrderBy(it => it.Order).ToList();
            var model = new DetailsMovieDTO();
            model.Movie = movie;
            model.Genres = movie.MoviesGenres.Select(it => it.Genre).ToList();
            model.Actors = movie.MoviesActors.Select(it =>

                new Person
                {
                    Name = it.Person.Name,
                    Picture = it.Person.Picture,
                    Character = it.Character,
                    Id = it.PersonId
                }
            ).ToList();

            model.UserVote = userVote;
            model.AverageVote = voteAvg;
            return model;

        }

        [HttpGet("update/{id}")]
        public async Task<ActionResult<MovieUpdateDto>> PutGet(int id)
        {
            var movieActionResult = await Get(id);
            if (movieActionResult.Result is NotFoundResult)
                return NotFound();

            var movieDetailsDto = movieActionResult.Value;
            var selectedGenresIds = movieDetailsDto.Genres.Select(it => it.Id).ToList();
            var notSelectedGenres = await _context.Genres.Where(it => !selectedGenresIds.Contains(it.Id)).ToListAsync();

            var model = new MovieUpdateDto();
            model.Movie = movieDetailsDto.Movie;
            model.SelectedGenres = movieDetailsDto.Genres;
            model.NotSelectedGenres = notSelectedGenres;
            model.Actors = movieDetailsDto.Actors;

            return model;

        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Movie movie)
        {
            var movieDb = await _context.Movies.FirstOrDefaultAsync(it => it.Id == movie.Id);

            if (movieDb == null)
                return NotFound();

            movieDb = _mapper.Map(movie, movieDb);

            if (!string.IsNullOrEmpty(movie.Poster))
            {
                var poster = Convert.FromBase64String(movie.Poster);
                movieDb.Poster = await _fileService.EditFile(poster, "jpg", "movies", movie.Poster);
            }


            await _context.Database.ExecuteSqlInterpolatedAsync($"delete from MoviesActors where MovieId = {movie.Id}; delete from MoviesGenres where MovieId = {movie.Id}");

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            movieDb.MoviesActors = movie.MoviesActors;
            movieDb.MoviesGenres = movie.MoviesGenres;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(it => it.Id == id);
            if (movie == null)
                return NotFound();

            _context.Remove(movie);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Movie>>> Filter(FilterMovieDto filterMovieDto)
        {
            var moviesQueryable = _context.Movies.AsQueryable();
            if (!string.IsNullOrEmpty(filterMovieDto.Title))
                moviesQueryable = moviesQueryable.Where(it => it.Title.Contains(filterMovieDto.Title));

            if(filterMovieDto.InTheaters)
                moviesQueryable = moviesQueryable.Where(it => it.InTheathers);

            if (filterMovieDto.UpcomingReleases)
                moviesQueryable = moviesQueryable.Where(it => it.ReleaseDate > DateTime.Today);

            if (filterMovieDto.GenreId != 0)
                moviesQueryable = moviesQueryable.Where(it => it.MoviesGenres.Select(it => it.GenreId).Contains(filterMovieDto.GenreId));

            await HttpContext.InsertPaginationParametersInResponse(moviesQueryable, filterMovieDto.RecordsPerPage);

            var movies = await moviesQueryable.Paginate(filterMovieDto.Pagination).ToListAsync();

            return movies;


        }

    }
}
