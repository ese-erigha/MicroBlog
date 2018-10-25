using System;
using MicroBlog.Core;
using MicroBlog.Repositories.Interfaces;

namespace MicroBlog.Repositories.Implementations
{
	public class PostRepository: GenericRepository<Entities.Post>, IPostRepository
    {
        public PostRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
