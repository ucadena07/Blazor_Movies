using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await _context.Genres.ToListAsync();
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Genre genre)
        {
            _context.Attach(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(it => it.Id == id);
            if (genre == null)
                return NotFound();
            return genre;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(it => it.Id == id);
            if (genre == null)
                return NotFound();

            _context.Remove(genre);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
