using System;
namespace MicroBlog.Models.ViewModels
{
    public class BaseViewModel
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
