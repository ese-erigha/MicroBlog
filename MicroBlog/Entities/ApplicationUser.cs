using System;
using System.Collections.Generic;
using MicroBlog.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MicroBlog.Entities
{
    public class ApplicationUser : IdentityUser<long>, ISoftDeletable
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IEnumerable<Post> Posts { get; set; } = new List<Post>();

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

    }
}
