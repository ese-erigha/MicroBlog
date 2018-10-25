using System;
namespace MicroBlog.Models.ViewModels
{
    public class CommentInfoViewModel : BaseViewModel
    {
        public string Description { get; set; }

        public long UserId { get; set; }

        public UserInfoViewModel User { get; set; }

        public long PostId { get; set; }

        public long Claps { get; set; }
    }
}
