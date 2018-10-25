using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroBlog.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public bool Disable { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Disable) return;
            if (!context.ModelState.IsValid) context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}
