using BlazorMovies.Shared.Repository.IRepository;
using BlazorMovies.SharedBackend.Helpers;
using BlazorMovies.SharedBackend.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace BlazorMovies.SharedBackend
{
    public static class IServiceCollectionBlazorMovies
    {
        public static IServiceCollection AddBlazorMovies(this IServiceCollection services)
        {
            services.AddScoped<IMovieRepository, MoviesRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(new[] { typeof(AutoMapperProfiles).Assembly });
            services.AddScoped<IFileService, FileService>();
            return services;
        }
    }
}
