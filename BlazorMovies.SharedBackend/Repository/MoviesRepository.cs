using AutoMapper;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
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

        public MoviesRepository(ApplicationDbContext context)
        {
            _context = context;

        }
        public Task<int> CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<DetailsMovieDTO> GetDetailsDTO(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IndexPageDTO> GetIndexPageDto()
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

        public Task<MovieUpdateDto> GetMovieForUpdate(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMovieDto filterMovieDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
