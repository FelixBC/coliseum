using AutoMapper;
using Coliseum.Api.DTOs;
using Coliseum.Api.Models;

namespace Coliseum.Api.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        // PostEntity -> PostDto
        CreateMap<PostEntity, PostDto>()
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Media.Select(m => m.Url).ToList()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags?.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList() ?? new List<string>()));

        // PostDto -> PostEntity
        CreateMap<PostDto, PostEntity>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => string.Join(",", src.Tags ?? new List<string>())));
    }
}
