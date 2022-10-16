using BlazorMovies.Shared.Dtos;

namespace BlazorMovies.Shared.Repository.IRepository
{
    public interface IUserRepository
    {
        Task AssignRole(EditRoleDto editRole);
        Task<List<RoleDto>> GetRoles();
        Task<PaginatedResponse<List<UserDto>>> GetUsers(PaginationDto paginationDto);
        Task RemoveRole(EditRoleDto editRole);
 
    }
}
