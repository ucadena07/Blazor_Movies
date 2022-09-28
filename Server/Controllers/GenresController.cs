using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovies.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GenresController(ApplicationDbContext context)
        {
            _context = context; 
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Genre genre)
        {
            _context.Add(genre);
            await _context.SaveChangesAsync();
            return genre.Id;
        }
    }
}
