using System;
namespace MicroBlog.Models.ViewModels
{
    public class PostInfoViewModel : BaseViewModel
    {

        public string Title { get; set; }

        public string Synopsis { get; set; }

        public string Image { get; set; }

        public string Content { get; set; }

        public UserInfoViewModel User { get; set; }

        public long UserId { get; set; }

        public long CommentCount { get; set; }

        public long Claps { get; set; }

    }
}
