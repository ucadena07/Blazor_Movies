using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlazorMovies.Server.Helpers
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<IdentityUser> _userClaimsPrincipalFactory;
        private readonly UserManager<IdentityUser> _userManager;
        public IdentityProfileService(IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory, UserManager<IdentityUser> userManager)
        {
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager; 
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(userId);
            var claimsPrincipal = await _userClaimsPrincipalFactory.CreateAsync(user);
            var claims = claimsPrincipal.Claims.ToList();

            var claimsDb = await _userManager.GetClaimsAsync(user);

            var mapppedClaims = new List<Claim>();

            foreach (var claim in claimsDb)
            {
                if(claim.Type == ClaimTypes.Role)
                {
                    mapppedClaims.Add(new Claim(JwtClaimTypes.Role, claim.Value));
                }
                else
                {
                    mapppedClaims.Add(claim);
                }
            }

            claims.AddRange(mapppedClaims);
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(userId);
            context.IsActive = user != null;
        }
    }
}
