using System;
using AutoMapper;
using MicroBlog.Models.ViewModels;
using MicroBlog.Entities;
using System.Linq;

namespace MicroBlog.Profiles
{
    public class EntityToViewModel : Profile
    {
        public EntityToViewModel()
        {
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<ApplicationUser, UserInfoViewModel>();
            CreateMap<Post, PostViewModel>();
            CreateMap<Post, PostInfoViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count()));

            CreateMap<Comment, CommentInfoViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        }
    }
}
