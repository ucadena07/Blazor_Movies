using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.SharedBackend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenreRepository _genreRepo;
        public GenresController(ApplicationDbContext context, IGenreRepository genreRepository)
        {
            _context = context;
            _genreRepo = genreRepository;   
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Genre genre)
        {
            await _genreRepo.CreateGenre(genre);
            return genre.Id;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await _genreRepo.GetGenres();
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Genre genre)
        {
            await _genreRepo.UpdateGenre(genre);
            return NoContent();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            var genre = await _genreRepo.GetGenre(id);
            if (genre == null) { return NotFound(); }
            return genre;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _genreRepo.GetGenre(id);
            if (genre == null)
            {
                return NotFound();
            }

            await _genreRepo.DeleteGenre(id);
            return NoContent();
        }
    }
}
