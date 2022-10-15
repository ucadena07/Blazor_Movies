using BlazorMovies.Client.Helpers;
using BlazorMovies.Client.Repository.IRepository;
using BlazorMovies.Shared.Dtos;
using System;

namespace BlazorMovies.Client.Repository
{
    //public class AccountsRepository : IAccountsRepository
    //{
    //    private readonly IHttpService _httpService;
    //    private readonly string baseUrl = "api/Account";
    //    public AccountsRepository(IHttpService httpService)
    //    {
    //        _httpService = httpService;
    //    }

    //    public async Task<UserToken> Register(UserInfo userInfo)
    //    {
    //        var httpResponse = await _httpService.Post<UserInfo, UserToken>($"{baseUrl}/create", userInfo);

    //        if (!httpResponse.Success)
    //        {
    //            throw new ApplicationException(await httpResponse.GetBody());
    //        }

    //        return httpResponse.Response;
    //    }

    //    public async Task<UserToken> Login(UserInfo userInfo)
    //    {
    //        var httpResponse = await _httpService.Post<UserInfo, UserToken>($"{baseUrl}/login", userInfo);

    //        if (!httpResponse.Success)
    //        {
    //            throw new ApplicationException(await httpResponse.GetBody());
    //        }

    //        return httpResponse.Response;
    //    }

    //    public async Task<UserToken> RenewToken()
    //    {
    //        var resp = await _httpService.Get<UserToken>($"{baseUrl}/RenewToken");
    //        if (!resp.Success)
    //        {
    //            throw new ApplicationException(await resp.GetBody());

    //        }
    //        return resp.Response;
    //    }
    //}
}
