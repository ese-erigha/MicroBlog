using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MicroBlog.Entities;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Services.Implementations
{
    public class AccountService : IAccountService
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;
        readonly RoleManager<IdentityRole<long>> _roleManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole<long>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password)
        {
            return await _signInManager.PasswordSignInAsync(user, password, false, false);
        }

        public async Task<bool> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }


        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            IdentityResult result = null;
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                result = await _roleManager.CreateAsync(new IdentityRole<long>() { Name = roleName });
            }

            result = await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }

        public async Task<ApplicationUser> FindByIdAsync(long id)
        {
            var user = await GetAllUsers().FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public IQueryable<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users;
        }

        public IQueryable<ApplicationUser> GetUsersByAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return GetAllUsers().Where(predicate);
        }

        public async Task<ApplicationUser> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user)
        {
            ApplicationUser entityUser = await _userManager.GetUserAsync(user);
            return entityUser;
        }

        public async Task<List<string>> GetRolesAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        {
            var result = await _userManager.IsEmailConfirmedAsync(user);
            return result;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return code;
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result;
        }
    }
}
