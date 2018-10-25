using System;
using System.Collections.Generic;

namespace MicroBlog.Models.ViewModels
{
    public class PostPagesViewModel
    {
        public PostInfoViewModel Post { get; set; }

        public IEnumerable<PostInfoViewModel> Posts { get; set; } = new List<PostInfoViewModel>();

        public PaginationViewModel PostPager { get; set; }

        public IEnumerable<CommentInfoViewModel> Comments { get; set; }

        public PaginationViewModel CommentsPager { get; set; }
    }
}
