using System;
using AutoMapper;
using MicroBlog.Models.ViewModels;
using MicroBlog.Entities;

namespace MicroBlog.Profiles
{
    public class ViewModelToEntity : Profile
    {
        public ViewModelToEntity()
        {
            CreateMap<RegisterViewModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<UserViewModel, ApplicationUser>();
            CreateMap<PostViewModel, Post>();

        }
    }
}
