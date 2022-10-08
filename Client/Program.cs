using BlazorMovies.Client;
using BlazorMovies.Client.Auth;
using BlazorMovies.Client.Helpers;
using BlazorMovies.Client.Repository;
using BlazorMovies.Client.Repository.IRepository;
using BlazorMovies.Client.Services;
using BlazorMovies.Client.Services.IService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<SingletonService>();
builder.Services.AddTransient<TransientService>();
builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IMovieRepository, MoviesRepository>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, DummyAuthenticationStateProvider>();
await builder.Build().RunAsync();
