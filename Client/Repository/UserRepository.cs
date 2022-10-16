using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpService _httpService;
        private string url = "api/users";
        public UserRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<PaginatedResponse<List<UserDto>>> GetUsers(PaginationDto paginationDto)
        {
            return await _httpService.GetHelper<List<UserDto>>(url, paginationDto);

        }


        public async Task<List<RoleDto>> GetRoles()
        {
            return await _httpService.GetHelper<List<RoleDto>>($"{url}/roles");
        }

        public async Task AssignRole(EditRoleDto editRole)
        {
            var response = await _httpService.Post($"{url}/assignRole", editRole);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task RemoveRole(EditRoleDto editRole)
        {
            var response = await _httpService.Post($"{url}/removeRole", editRole);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

   
    }
}
