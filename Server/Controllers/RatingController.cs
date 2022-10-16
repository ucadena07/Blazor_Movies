using BlazorMovies.Shared.Entities;
using BlazorMovies.SharedBackend;
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
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        public RatingController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Rate(MovieRating movieRating)
        {
            var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var userId = user.Id;

            var currentRating = await _context.MovieRatings.FirstOrDefaultAsync(it => it.MovieId == movieRating.MovieId && it.UserId == userId);

            if(currentRating == null)
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

            return NoContent();
        }
    }
}
