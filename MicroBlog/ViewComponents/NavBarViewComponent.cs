using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroBlog.Models.ViewModels;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {

        readonly IAccountService _accountService;
        public NavBarViewComponent(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _accountService.GetCurrentUserAsync(HttpContext.User);
            List<string> userRoles = await _accountService.GetRolesAsync(user);
            UserProfileViewModel model = new UserProfileViewModel
            {
                User = user,
                Roles = userRoles
            };
            return View(model);
        }
    }
}
