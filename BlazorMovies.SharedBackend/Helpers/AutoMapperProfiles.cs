using AutoMapper;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.SharedBackend.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Person, Person>()
                .ForMember(it => it.Picture, option => option.Ignore());

            CreateMap<Movie, Movie>()
            .ForMember(it => it.Poster, option => option.Ignore());
        }
    }
}
