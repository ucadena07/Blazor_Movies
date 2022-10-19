using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Server.Helpers.Interfaces;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.SharedBackend;
using BlazorMovies.SharedBackend.Helpers;
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
    public class MoviesController : ControllerBase
    {
        private IMovieRepository _movieRepository;
        public MoviesController(IMovieRepository movieRepository)
        {

            _movieRepository = movieRepository;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {

            return await _movieRepository.CreateMovie(movie);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            return await _movieRepository.GetIndexPageDto();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DetailsMovieDTO>> Get(int id)
        {

            var model = await _movieRepository.GetDetailsDTO(id);
            if (model == null)
                return NotFound();
            return model;
        }

        [HttpGet("update/{id}")]
        public async Task<ActionResult<MovieUpdateDto>> PutGet(int id)
        {
            var model = await _movieRepository.GetMovieForUpdate(id);
            if (model == null) return NotFound();
            return model;

        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Movie movie)
        {
            var movieDb = await _movieRepository.GetDetailsDTO(movie.Id);
            if(movieDb == null) return NotFound();
            await _movieRepository.UpdateMovie(movie);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetDetailsDTO(id);
            if (movie == null)
                return NotFound();
            await _movieRepository.DeleteMovie(id);
  
            return NoContent();
        }

        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Movie>>> Filter(FilterMovieDto filterMovieDto)
        {

            var paginatedRespo = await _movieRepository.GetMoviesFiltered(filterMovieDto);
            HttpContext.InsertPaginationParametersInResponse(paginatedRespo.TotalAmountPages);
            return paginatedRespo.Response;
        }

    }
}
