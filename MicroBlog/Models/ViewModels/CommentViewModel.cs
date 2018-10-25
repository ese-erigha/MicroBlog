using System;
using System.ComponentModel.DataAnnotations;

namespace MicroBlog.Models.ViewModels
{
    public class CommentViewModel
    {
        [Required(ErrorMessage="Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "User is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Post is required")]
        public long PostId { get; set; }
    }
}
