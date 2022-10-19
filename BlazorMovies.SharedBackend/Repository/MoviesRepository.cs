using AutoMapper;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.SharedBackend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.SharedBackend.Repository
{
    public class MoviesRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticationStateService _authStateService;
        private readonly IFileService _fileService;
        private IMapper _mapper;

        public MoviesRepository(ApplicationDbContext context, IAuthenticationStateService stateService, IFileService fileService, IMapper mapper)
        {
            _context = context;
            _authStateService = stateService;
            _fileService = fileService;
            _mapper = mapper;   
        }
        public async Task<int> CreateMovie(Movie movie)
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

        public async Task DeleteMovie(int Id)
        {
            var movie = await _context.Movies.FindAsync(Id);
            _context.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<DetailsMovieDTO> GetDetailsDTO(int id)
        {
            var movie = await _context.Movies.Where(it => it.Id == id)
                 .Include(it => it.MoviesGenres).ThenInclude(it => it.Genre)
                 .Include(it => it.MoviesActors).ThenInclude(it => it.Person)
                 .FirstOrDefaultAsync();
            if (movie == null)
            {
                return null;
            }

            var voteAvg = 0.0;
            var userVote = 0;

            if (await _context.MovieRatings.AnyAsync(it => it.MovieId == id))
            {
                voteAvg = await _context.MovieRatings.Where(it => it.MovieId == id).AverageAsync(it => it.Rate);

                var userId = await _authStateService.GetCurrentUserId();

                if (userId != null)
                {
 

                    var userVoteDb = await _context.MovieRatings.FirstOrDefaultAsync(it => it.MovieId == id && it.UserId == userId);

                    if (userVoteDb != null)
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

        public async Task<IndexPageDTO> GetIndexPageDto()
        {
            var limit = 6;

            var testId = await _authStateService.GetCurrentUserId();

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

        public async Task<MovieUpdateDto> GetMovieForUpdate(int id)
        {
            var movieDetailsDto = await GetDetailsDTO(id);
            if(movieDetailsDto == null) return null;

            var selectedGenresIds = movieDetailsDto.Genres.Select(it => it.Id).ToList();
            var notSelectedGenres = await _context.Genres.Where(it => !selectedGenresIds.Contains(it.Id)).ToListAsync();

            var model = new MovieUpdateDto();
            model.Movie = movieDetailsDto.Movie;
            model.SelectedGenres = movieDetailsDto.Genres;
            model.NotSelectedGenres = notSelectedGenres;
            model.Actors = movieDetailsDto.Actors;

            return model;
        }

        public async Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMovieDto filterMovieDto)
        {
            var moviesQueryable = _context.Movies.AsQueryable();
            if (!string.IsNullOrEmpty(filterMovieDto.Title))
                moviesQueryable = moviesQueryable.Where(it => it.Title.Contains(filterMovieDto.Title));

            if (filterMovieDto.InTheaters)
                moviesQueryable = moviesQueryable.Where(it => it.InTheathers);

            if (filterMovieDto.UpcomingReleases)
                moviesQueryable = moviesQueryable.Where(it => it.ReleaseDate > DateTime.Today);

            if (filterMovieDto.GenreId != 0)
                moviesQueryable = moviesQueryable.Where(it => it.MoviesGenres.Select(it => it.GenreId).Contains(filterMovieDto.GenreId));



            var movies = await moviesQueryable.GetPaginatedResponse(filterMovieDto.Pagination);

            return movies;

        }

        public async Task UpdateMovie(Movie movie)
        {
            _context.Entry(movie).State = EntityState.Detached;
            var movieDb = await _context.Movies
                .Include(it => it.MoviesActors)
                .Include(it  => it.MoviesGenres)
                .FirstOrDefaultAsync(it => it.Id == movie.Id);


            movieDb = _mapper.Map(movie, movieDb);

            if (!string.IsNullOrEmpty(movie.Poster))
            {
                var poster = Convert.FromBase64String(movie.Poster);
                movieDb.Poster = await _fileService.EditFile(poster, "jpg", "movies", movie.Poster);
            }




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
        }
    }
}
