using System;
using System.Linq;
using MicroBlog.Core.Interfaces;
using MicroBlog.Models.ViewModels;
using MicroBlog.Repositories.Interfaces;
using MicroBlog.Services.Interfaces;

namespace MicroBlog.Services.Implementations
{
    public class CommentService : EntityService<Entities.Comment>, ICommentService
    {
        readonly ICommentRepository _repository;

        public CommentService(IUnitOfWork unitOfWork, ICommentRepository repository) : base(unitOfWork, repository)
        {
            _repository = repository;
        }

        public IQueryable<CommentInfoViewModel> GetCommentsWithUser()
        {
            return GetAll().Select(comment => new CommentInfoViewModel
            {
                Id = comment.Id,
                Description = comment.Description,
                UserId = comment.UserId,
                User = new UserInfoViewModel{
                    Id = comment.User.Id,
                    FirstName = comment.User.FirstName,
                    LastName = comment.User.LastName,
                    Email = comment.User.Email,
                    PhoneNumber =comment.User.PhoneNumber
                },
                PostId = comment.PostId,
                Claps = comment.Claps,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            });
        }
    }
}
