using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MicroBlog.Entities;
using Microsoft.AspNetCore.Identity;

namespace MicroBlog.Services.Interfaces
{
    public interface IAccountService : IService
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        Task<bool> SignOutAsync();

        Task<ApplicationUser> FindByEmailAsync(string email);

        Task<ApplicationUser> FindByIdAsync(long Id);

        Task<IdentityResult> UpdateAsync(ApplicationUser user);

        Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password);

        Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName);

        IQueryable<ApplicationUser> GetAllUsers();

        IQueryable<ApplicationUser> GetUsersByAsync(Expression<Func<ApplicationUser, bool>> predicate);

        Task<ApplicationUser> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user);

        Task<List<string>> GetRolesAsync(ApplicationUser user);

        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);

        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);

        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);

        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
    }
}
