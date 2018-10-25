using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MicroBlog.Entities;
using MicroBlog.Services.Interfaces;

namespace MicroBlog.Models.Dto.CreateDto
{
    public class CommentDto : IValidatableObject
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "User is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Post is required")]
        public long PostId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            validationResults = ValidateUser(validationContext, validationResults);

            validationResults = ValidatePost(validationContext, validationResults);

            return validationResults;
        }

        public List<ValidationResult> ValidateUser(ValidationContext validationContext, List<ValidationResult> validationResults)
        {
            IAccountService accountService = (IAccountService)validationContext.GetService(typeof(IAccountService));

            ApplicationUser user = accountService.GetAllUsers().Where(x => x.Id == UserId).SingleOrDefault();

            if (user == null)
            {
                validationResults.Add(new ValidationResult("User does not exist", new string[] { "UserId" }));
            }
            return validationResults;
        }

        public List<ValidationResult> ValidatePost(ValidationContext validationContext, List<ValidationResult> validationResults)
        {
            IPostService postService = (IPostService)validationContext.GetService(typeof(IPostService));

            Post post = postService.GetAll().Where(x => x.Id == PostId).SingleOrDefault();

            if (post == null)
            {
                validationResults.Add(new ValidationResult("Post does not exist", new string[] { "PostId" }));
            }
            return validationResults;
        }

    }
}
