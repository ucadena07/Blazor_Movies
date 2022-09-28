﻿using BlazorMovies.Client.Helpers;
using BlazorMovies.Client.Repository.IRepository;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly IHttpService _httpService;
        private string url = "api/genres";
        public GenreRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task CreateGenre(Genre genre)
        {
            var response = await _httpService.Post(url, genre);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
