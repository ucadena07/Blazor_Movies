using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.SharedBackend;
using BlazorMovies.SharedBackend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get([FromQuery] PaginationDto paginationDto)
        {
            var paginatedResp = await _userRepository.GetUsers(paginationDto);
            HttpContext.InsertPaginationParametersInResponse(paginatedResp.TotalAmountPages);
            return paginatedResp.Response;
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleDto>>> Get()
        {
            return await _userRepository.GetRoles();
        }

        [HttpPost("assignRole")]
        public async Task<ActionResult> AssignRole(EditRoleDto editRoleDto)
        {
            await _userRepository.AssignRole(editRoleDto);
            return NoContent();
        }

        [HttpPost("removeRole")]
        public async Task<ActionResult> RemoveRole(EditRoleDto editRoleDto)
        {
            await _userRepository.RemoveRole(editRoleDto);
            return NoContent();
        }

    }
}
