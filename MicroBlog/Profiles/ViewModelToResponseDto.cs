using System;
using AutoMapper;
using MicroBlog.Models.Dto.ResponseDto;
using MicroBlog.Models.ViewModels;

namespace MicroBlog.Profiles
{
    public class ViewModelToResponseDto : Profile
    {
        public ViewModelToResponseDto()
        {
            CreateMap<UserInfoViewModel, UserDto>();

            CreateMap<CommentInfoViewModel, CommentDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
        }
    }
}
