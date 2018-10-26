using System;
using System.Threading.Tasks;
using MicroBlog.Entities;
using MicroBlog.Helpers;
using SendGrid.Helpers.Mail;

namespace MicroBlog.Services.Interfaces
{
    public interface IEmailService : IService
    {
        void SendEmailConfirmation(ApplicationUser user, string url);

        void SendResetPasswordRequestEmail(ApplicationUser user, string url);
    }
}
