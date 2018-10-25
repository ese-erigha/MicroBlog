using System;
namespace MicroBlog.Models.ViewModels
{
	public class UserInfoViewModel : BaseViewModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
