using BlazorMovies.Client;
using BlazorMovies.Client.Auth;
using BlazorMovies.Client.Helpers;
using BlazorMovies.Client.Repository;
using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.Client.Services;
using BlazorMovies.Client.Services.IService;
using BlazorMovies.SharedComponents;
using BlazorMovies.SharedComponents.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.IdentityModel.Tokens.Jwt;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<HttpClientWithToken>(client =>  client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient<HttpClientWIthOutToken>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddLocalization();

builder.Services.AddSingleton<SingletonService>();
builder.Services.AddTransient<TransientService>();
builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IMovieRepository, MoviesRepository>();
builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IDisplayMessage, DisplayMessage>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IExampleInterface, ExampleImplementation>();
//builder.Services.AddScoped<TokenRenewer>();
//builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped<JwtAuthenticationStateProvider>();
//builder.Services.AddScoped<AuthenticationStateProvider, DummyAuthenticationStateProvider>();

//builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>(
//provider => provider.GetRequiredService<JwtAuthenticationStateProvider>());

//builder.Services.AddScoped<ILoginService, JwtAuthenticationStateProvider>(   
// provider => provider.GetRequiredService<JwtAuthenticationStateProvider>());
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();



builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
