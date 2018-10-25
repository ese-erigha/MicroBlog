using System;
namespace MicroBlog.Models.Dto.ResponseDto
{
    public class UserDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string Email { get; set; }


        public string PhoneNumber { get; set; }
    }
}
