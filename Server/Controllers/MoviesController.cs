using BlazorMovies.Server.Helpers.Interfaces;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileAzureService _fileAzureService;
        private readonly IFileService _fileService;
        public MoviesController(ApplicationDbContext context, IFileAzureService fileAzureService, IFileService fileService)
        {
            _context = context;
            _fileAzureService = fileAzureService;
            _fileService = fileService;
        }

        public IFileService FileService { get; }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            if (!string.IsNullOrEmpty(movie.Poster))
            {
                var porster = Convert.FromBase64String(movie.Poster);
                //person.Picture = await _fileAzureService.SaveFile(personPicture, ".jpg", "people");
                movie.Poster = await _fileService.SaveFile(porster, ".jpg", "people");
            }

            _context.Add(movie);
            await _context.SaveChangesAsync();
            return movie.Id;
        }


    }
}
