using System;
using System.Collections.Generic;
using MicroBlog.Entities;

namespace MicroBlog.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public ApplicationUser User { get; set; }

        public List<string> Roles { get; set; }
    }
}
