using System;
using AutoMapper;
using MicroBlog.Entities;
using MicroBlog.Models.Dto.CreateDto;

namespace MicroBlog.Profiles
{
    public class CreateDtoToEntity : Profile
    {
        public CreateDtoToEntity()
        {
            CreateMap<UserDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<CommentDto, Comment>();
        }
    }
}
