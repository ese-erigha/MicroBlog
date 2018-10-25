using System;
using System.Linq;
using MicroBlog.Models.ViewModels;
using MicroBlog.Entities;

namespace MicroBlog.Services.Interfaces
{
    public interface IPostService : IEntityService<Entities.Post>
    {
        IQueryable<PostInfoViewModel> GetPostsWithCommentCount();

        Post GetPostWithComments(long id);
    }
}
