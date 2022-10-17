using BlazorMovies.SharedBackend.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace BlazorMovies.ServerSide2.Helpers
{
    public class AuthenticationStateServiceServerSide : IAuthenticationStateService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthenticationStateServiceServerSide(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider; 
        }
        public async Task<string> GetCurrentUserId()
        {
            var userState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (!userState.User.Identity.IsAuthenticated)
                return null;

            var claims = userState.User.Claims;

            var claimsWithUserId = claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier);

            if (claimsWithUserId == null)
                throw new ApplicationException("Could not find the Users'Ids");

            return claimsWithUserId.Value;
        }
    }
}
