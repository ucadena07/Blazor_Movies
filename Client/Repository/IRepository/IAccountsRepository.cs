using BlazorMovies.Shared.Dtos;

namespace BlazorMovies.Client.Repository.IRepository
{
    public interface IAccountsRepository
    {
        Task<UserToken> Login(UserInfo userInfo);
        Task<UserToken> Register(UserInfo userInfo);
    }
}
