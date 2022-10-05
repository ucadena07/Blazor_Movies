using AutoMapper;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Server.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Person, Person>()
                .ForMember(it => it.Picture, option => option.Ignore());
        }
    }
}
