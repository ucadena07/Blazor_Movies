﻿using BlazorMovies.Client.Helpers;
using BlazorMovies.Client.Repository.IRepository;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IHttpService _httpService;
        private string url = "api/people";
        public PersonRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task CreatePerson(Person person)
        {
            var response = await _httpService.Post(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}