using BlazorMovies.Client.Helpers;
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

        public async Task<List<Person>> GetPeople()
        {
            var response = await _httpService.Get<List<Person>>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;

        }
        public async Task CreatePerson(Person person)
        {
            var response = await _httpService.Post(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task<List<Person>> GetPeopleByName(string name)
        {
            var response = await _httpService.Get<List<Person>>($"{url}/search/{name}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task UpdatePerson(Person person)
        {
            var response = await _httpService.Put(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }


        public async Task<Person> GetPersonById(int id)
        {
            return await _httpService.GetHelper<Person>($"{url}/{id}");

        }

        public async Task DeletePerson(int Id)
        {
            var response = await _httpService.Delete<Person>($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
