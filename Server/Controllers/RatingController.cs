using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repository.IRepository;
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
        private readonly IRatingRepository _ratingRepository;
        public RatingController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Rate(MovieRating movieRating)
        {
            await _ratingRepository.Vote(movieRating);
            return NoContent();
        }
    }
}
