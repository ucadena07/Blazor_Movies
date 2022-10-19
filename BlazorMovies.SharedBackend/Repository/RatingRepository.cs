using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.SharedBackend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.SharedBackend.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IAuthenticationStateService _authStateService;
        private readonly ApplicationDbContext _context;
        public RatingRepository(IAuthenticationStateService stateService, ApplicationDbContext context)
        {
            _authStateService = stateService;
            _context = context;
        }
        public async Task Vote(MovieRating movieRating)
        {
            var userId = await _authStateService.GetCurrentUserId();
            if (userId == null) return;

            var currentRating = await _context.MovieRatings.FirstOrDefaultAsync(it => it.MovieId == movieRating.MovieId && it.UserId == userId);

            if (currentRating == null)
            {
                movieRating.UserId = userId;
                movieRating.RatingDate = DateTime.Today;
                _context.Add(movieRating);
                await _context.SaveChangesAsync();
            }
            else
            {
                currentRating.Rate = movieRating.Rate;
                await _context.SaveChangesAsync();
            }
        }
    }
}
