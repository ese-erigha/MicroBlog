using System;
namespace MicroBlog.Models.Dto.ResponseDto
{
    public class CommentDto : BaseDto
    {
        public string Description { get; set; }

        public UserDto User { get; set; }

        public long PostId { get; set; }

        public long Claps { get; set; }
    }
}
