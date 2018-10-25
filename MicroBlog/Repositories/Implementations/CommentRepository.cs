using System;
using MicroBlog.Core;
using MicroBlog.Repositories.Interfaces;

namespace MicroBlog.Repositories.Implementations
{
    public class CommentRepository: GenericRepository<Entities.Comment>, ICommentRepository
    {
        public CommentRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
