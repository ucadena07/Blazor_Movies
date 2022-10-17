using BlazorMovies.SharedBackend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Helpers
{
    public class AuthenticationStateServiceWebAssembly : IAuthenticationStateService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        public AuthenticationStateServiceWebAssembly(IHttpContextAccessor httpContext, UserManager<IdentityUser> userManager)
        {
            _httpContext = httpContext;
            _userManager = userManager;
        }
        public async Task<string> GetCurrentUserId()
        {
            if (!_httpContext.HttpContext.User.Identity.IsAuthenticated)
                return null;

            var user = await _userManager.FindByEmailAsync(_httpContext.HttpContext.User.Identity.Name);
            var userId = user.Id;
            return userId;  
        }
    }
}
