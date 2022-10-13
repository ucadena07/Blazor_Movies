﻿using BlazorMovies.Shared.Dtos;

namespace BlazorMovies.Client.Auth
{
    public interface ILoginService
    {
        Task Login(UserToken userToken);
        Task Logout();
        Task TryRenewToken();
    }
}
