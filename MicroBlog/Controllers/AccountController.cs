using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MicroBlog.Entities;
using MicroBlog.Filters;
using MicroBlog.Helpers;
using MicroBlog.Models.ViewModels;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CreateDto = MicroBlog.Models.Dto.CreateDto;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroBlog.Controllers
{
    public class AccountController : Controller
    {
        readonly IMapper _mapper;
        readonly IAccountService _accountService;
        readonly IPaginationService _paginationService;
        readonly IEmailService _emailService;

        public AccountController(IMapper mapper, IAccountService accountService, IPaginationService paginationService, IEmailService emailService)
        {
            _mapper = mapper;
            _accountService = accountService;
            _paginationService = paginationService;
            _emailService = emailService;
        }

        [Route("auth/login", Name = "login")]
        public IActionResult Login([FromQuery(Name = "ReturnUrl")] string returnUrl = "/")
        {
            LoginViewModel model = new LoginViewModel() { ReturnUrl = returnUrl ?? "/" };
            return View(model);
        }
 

        [Route("auth/logout", Name = "logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _accountService.SignOutAsync();
            return RedirectToAction("login");
        }

        

        [HttpPost("auth/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _accountService.FindByEmailAsync(model.Email);

                if (user != null)
                {

                    bool isEmailConfirmed = await _accountService.IsEmailConfirmedAsync(user);
                    if (!isEmailConfirmed)
                    {
                        ModelState.AddModelError("", "Email not confirmed!");
                        return View(model);
                    }

                    SignInResult result = await _accountService.PasswordSignInAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }

                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Too many attempts. please try again after 5 minutes");
                    }

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid user name or password");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid user name or password");
                }
            }

            return View(model);

        }


        [Route("user/register", Name = "user-register")]
        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost("user/register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRegistration(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = null;
                var user = await _accountService.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "User account already exists");
                }
                else
                {
                    ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(model);
                    result = await _accountService.CreateAsync(applicationUser, model.Password);

                    if (result.Succeeded)
                    {
                        result = await _accountService.AddUserToRoleAsync(applicationUser, "User");

                        if (result.Succeeded)
                        {
                            string confirmationToken = await _accountService.GenerateEmailConfirmationTokenAsync(applicationUser);

                            string confirmationLink = Url.Action("ConfirmEmail",
                                "Account", new
                                {
                                    userid = user.Id,
                                    token = confirmationToken
                                },
                                protocol: HttpContext.Request.Scheme);

                            _emailService.SendEmailConfirmation(applicationUser, confirmationLink);
                            ViewBag.Message = "An email has been sent to your account for confirmation";
                            return RedirectToAction("login");
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet("auth/confirm-password")]
        public async Task<IActionResult> ConfirmEmail(long userid, string token)
        {
            ApplicationUser user = await _accountService.FindByIdAsync(userid);
            IdentityResult result = await _accountService.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["Message"] = "Email confirmed successfully!";
                return RedirectToAction("Login");

            }
            else
            {
                TempData["Error"] = "Error while confirming your email!";
                return RedirectToAction("Login");
            }
        }




        [Route("admin/register", Name = "admin-register")]
        public IActionResult AdminRegistration()
        {
            return View();
        }

        [HttpPost("admin/register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminRegistration(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = null;
                var user = await _accountService.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "User account already exists");
                }
                else
                {
                    ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(model);
                    result = await _accountService.CreateAsync(applicationUser, model.Password);

                    if (result.Succeeded)
                    {
                        result = await _accountService.AddUserToRoleAsync(applicationUser, "Admin");

                        if (result.Succeeded)
                        {
                            return RedirectToAction("login");
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        //[Authorize(Roles="Admin")]
        [Authorize]
        [Route("user/list", Name = "user-list")]
        public async Task<IActionResult> Users([FromQuery]PaginationInfo paginationInfo)
        {
            var paginated = _paginationService.Paginate(_accountService.GetAllUsers(), paginationInfo);
            var currentUser = await _accountService.GetCurrentUserAsync(HttpContext.User);
            var userRoles = await _accountService.GetRolesAsync(currentUser);

            UserDetailsViewModel viewModel = new UserDetailsViewModel()
            {
                Pager = paginated.Pager,
                PagedUsers = _mapper.Map<IEnumerable<UserInfoViewModel>>(paginated.PagedItems)
            };
            return View(viewModel);
        }

        [HttpGet("user/account/{id}", Name = "GetUserById")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUserById(long id)
        {
            var userEntity = await _accountService.FindByIdAsync(id);
            if (userEntity == null)
            {
                return StatusCode(404);
            }

            var user = _mapper.Map<UserViewModel>(userEntity);
            return Ok(user);
        }

        [HttpPut("user/account/{id}")]
        [ValidateAntiForgeryToken]
        [ValidateModel(Disable = false)]
        public async Task<IActionResult> UpdateUser(long id, [FromBody]CreateDto.UserDto user)
        {
            var userEntity = await _accountService.FindByIdAsync(id);
            if (userEntity == null)
            {
                return StatusCode(404);
            }

            _mapper.Map(user, userEntity);
            userEntity.Id = id;

            var result = await _accountService.UpdateAsync(userEntity);
            return result.Succeeded ? (IActionResult)StatusCode(204) : StatusCode(500, "A problem occurred while handling your request");

        }


        [HttpGet("auth/forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("auth/forgot-password")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Account does not exist");
                    return View(model);
                }

                bool isEmailConfirmed = await _accountService.IsEmailConfirmedAsync(user);
                if (!isEmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email has not been confirmed. Please check your email and confirm your account");
                    return View(model);
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _accountService.GeneratePasswordResetTokenAsync(user);

                string passwordResetUrl = Url.Action("ResetPassword", "Account", new { userid = user.Id, token = code }, protocol: HttpContext.Request.Scheme);

                //Send Email with callback url
                _emailService.SendResetPasswordRequestEmail(user, passwordResetUrl);

                ViewBag.Message = "Password reset link has been sent to your email address!";
                return View();
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost("auth/reset-password", Name ="PasswordReset")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            ApplicationUser user = await _accountService.FindByEmailAsync(viewModel.Email);

            if (user == null)
            {
                TempData["Error"] = "You are not allowed to reset password";
                return RedirectToAction("Login");
            }

            IdentityResult result = await _accountService.ResetPasswordAsync(user, viewModel.PasswordResetToken, viewModel.Password);
            if (result.Succeeded)
            {
                TempData["Message"] = "Your password reset was successful!";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "Sorry.. Unable to reset your password";
            return RedirectToAction("ForgotPassword");
        }



        [HttpGet("auth/reset-password/{userid}/{token}")]
        public async Task<IActionResult> ResetPassword(long userid, string token)
        {
            ApplicationUser user = await _accountService.FindByIdAsync(userid);

            if (user == null)
            {
                TempData["Error"] = "You are not allowed to reset password";
                return RedirectToAction("Login");
            }

            var viewModel = new ResetPasswordViewModel() { PasswordResetToken = token, Email = user.Email };
            return View(viewModel);
        }


    }
}
