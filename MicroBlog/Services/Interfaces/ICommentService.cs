using System;
using System.Linq;
using MicroBlog.Models.ViewModels;

namespace MicroBlog.Services.Interfaces
{
    public interface ICommentService : IEntityService<Entities.Comment>
    {

        IQueryable<CommentInfoViewModel> GetCommentsWithUser();

        CommentInfoViewModel GetCommentWithUser(long userId);
    }
}
