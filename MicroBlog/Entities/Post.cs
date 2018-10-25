using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroBlog.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }

        public string Synopsis { get; set; }

        public string Image { get; set; }

        public string Content { get; set; }

        public ApplicationUser User { get; set; }

        public long UserId {get; set;}

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        public long Claps { get; set; }
    }
}
