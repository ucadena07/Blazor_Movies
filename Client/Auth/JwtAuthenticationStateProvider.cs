using BlazorMovies.SharedComponents.Helpers;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.Shared.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorMovies.Client.Auth
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider, ILoginService
    {
        private readonly IJSRuntime _js;
        private readonly string TOKENKEY = "TOKENKEY";
        private readonly string EXPTOKENKEY = "EXPTOKENKEY";
        private readonly HttpClient _httpClient;
        private readonly IAccountsRepository _accountRepo;

        private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        public JwtAuthenticationStateProvider(IJSRuntime js, HttpClient httpClient, IAccountsRepository accountsRepository)
        {
            _js = js;
            _httpClient = httpClient;
            _accountRepo = accountsRepository;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.GetFromLocalStorage(TOKENKEY);

            if (string.IsNullOrEmpty(token))
            {
                return Anonymous;
            }

            var expTimeString = await _js.GetFromLocalStorage(EXPTOKENKEY);
            DateTime expTime;

            if(DateTime.TryParse(expTimeString, out expTime))
            {
                if (isTokenExpired(expTime))
                {
                    await CleanUp();
                    return Anonymous;
                }

                if (ShouldRenewToken(expTime))
                {
                    token = await RenewToken(token);
                }
            }


            return BuildAuthenticationState(token);
            
        }

        private async Task<string> RenewToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var newToken = await _accountRepo.RenewToken();
            await _js.SetInLocalStorage(TOKENKEY, newToken.Token);
            await _js.SetInLocalStorage(EXPTOKENKEY, newToken.Expiration.ToString());
            return newToken.Token;
        }

        public async Task TryRenewToken()
        {
            var expTimeString = await _js.GetFromLocalStorage(EXPTOKENKEY);
            DateTime expTime;

            if (DateTime.TryParse(expTimeString, out expTime))
            {
                if (isTokenExpired(expTime))
                {
                    await Logout();
                   
                }

                if (ShouldRenewToken(expTime))
                {
                    var token = await _js.GetFromLocalStorage(TOKENKEY);
                    var newToken = await RenewToken(token);
                    var authState = BuildAuthenticationState(newToken);
                    NotifyAuthenticationStateChanged(Task.FromResult(authState));
                }
            }
        }

        private bool ShouldRenewToken(DateTime expirationTime)
        {
            return expirationTime.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(50);
        }

        private bool isTokenExpired(DateTime expirationTime)
        {
            return expirationTime <= DateTime.UtcNow;
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));

        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Login(UserToken userToken)
        {
            await _js.SetInLocalStorage(TOKENKEY, userToken.Token);
            await _js.SetInLocalStorage(EXPTOKENKEY, userToken.Expiration.ToString());
            var authState = BuildAuthenticationState(userToken.Token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await CleanUp();
            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
        }

        private async Task CleanUp()
        {
            await _js.RemoveItem(TOKENKEY);
            await _js.RemoveItem(EXPTOKENKEY);
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
