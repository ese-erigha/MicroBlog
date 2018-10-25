using System;
namespace MicroBlog.Models.Dto
{
    public class BaseDto
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
