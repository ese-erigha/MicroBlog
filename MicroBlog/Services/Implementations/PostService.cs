using System;
using System.Linq;
using MicroBlog.Core.Interfaces;
using MicroBlog.Entities;
using MicroBlog.Models.ViewModels;
using MicroBlog.Repositories.Interfaces;
using MicroBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Services.Implementations
{
    public class PostService : EntityService<Entities.Post>, IPostService
    {
        readonly IPostRepository _repository;

        public PostService(IUnitOfWork unitOfWork, IPostRepository repository): base(unitOfWork,repository)
        {
            _repository = repository;
        }

        public IQueryable<PostInfoViewModel> GetPostsWithCommentCount()
        {
            return GetAll().Select(post => new PostInfoViewModel
                            {
                                Id = post.Id,
                                Title = post.Title,
                                Image = post.Image,
                                Content = post.Content,
                                CommentCount = post.Comments.Count(),
                                CreatedAt = post.CreatedAt,
                                UpdatedAt = post.UpdatedAt
                            });
        }

        public Post GetPostWithComments(long id)
        {
            return GetAll()
                          .Include(p => p.Comments)
                                .ThenInclude(c => c.User)
                          .Include(p => p.User)
                   .SingleOrDefault(p => p.Id == id);

        }
    }
}
