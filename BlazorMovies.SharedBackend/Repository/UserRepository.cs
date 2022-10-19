using BlazorMovies.Shared.Dtos;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.SharedBackend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.SharedBackend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public UserRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }
        public async Task AssignRole(EditRoleDto editRoleDto)
        {
            var user = await _userManager.FindByIdAsync(editRoleDto.UserId);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDto.RoleName));
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            return await _context.Roles.Select(it => new RoleDto { RoleName = it.Name }).ToListAsync();
        }

        public async Task<PaginatedResponse<List<UserDto>>> GetUsers(PaginationDto paginationDto)
        {
            var queryable = _context.Users.AsQueryable();

            var paginatedResponse = await queryable.GetPaginatedResponse(paginationDto);
                
              

            var usersDTO = paginatedResponse.Response.Select(it => new UserDto { Email = it.Email, UserId = it.Id }).ToList();
            var paginatedRespDTO = new PaginatedResponse<List<UserDto>>()
            {
                Response = usersDTO,
                TotalAmountPages = paginatedResponse.TotalAmountPages
            };

            return paginatedRespDTO;
        }

        public async Task RemoveRole(EditRoleDto editRoleDto)
        {
            var user = await _userManager.FindByIdAsync(editRoleDto.UserId);
            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDto.RoleName));
        }
    }
}
