using BlazorMovies.Shared.Dtos;

namespace BlazorMovies.Shared.Repository.IRepository
{
    public interface IAccountsRepository
    {
        Task<UserToken> Login(UserInfo userInfo);
        Task<UserToken> Register(UserInfo userInfo);
        Task<UserToken> RenewToken();
    }
}
