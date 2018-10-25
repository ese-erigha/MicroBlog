using System;
using AutoMapper;
using MicroBlog.Entities;
using MicroBlog.Models.Dto.ResponseDto;

namespace MicroBlog.Profiles
{
    public class EntityToResponseDto : Profile
    {
        public EntityToResponseDto()
        {
            CreateMap<ApplicationUser, UserDto>();

            CreateMap<Comment, CommentDto>();
        }
    }
}
