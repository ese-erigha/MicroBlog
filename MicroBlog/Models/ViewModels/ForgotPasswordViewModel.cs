using System;
using System.ComponentModel.DataAnnotations;

namespace MicroBlog.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
