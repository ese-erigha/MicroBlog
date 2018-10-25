using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroBlog.Entities
{
    public class Comment : BaseEntity
    {
        public string Description { get; set; }

        public ApplicationUser User { get; set; }

        public long UserId { get; set; }

        public long PostId { get; set; }

        public Post Post { get; set; }

        public long Claps { get; set; }
    }
}
