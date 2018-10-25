using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Models.ViewModels
{
    public class PostViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Synopsis is required")]
        public string Synopsis { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        public long UserId { get; set; }
    }
}
