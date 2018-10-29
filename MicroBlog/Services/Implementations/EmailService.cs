using System;
using System.Threading.Tasks;
using Hangfire;
using MicroBlog.Entities;
using MicroBlog.Helpers;
using MicroBlog.Helpers.Interfaces;
using MicroBlog.Services.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using EmailAddress = MicroBlog.Helpers.EmailAddress;

namespace MicroBlog.Services.Implementations
{
    public class EmailService : IEmailService
    {
        readonly SendGridSettings _sendGridSettings;
        readonly SmtpSettings _smtpSettings;
        readonly IBackgroundJobClient _backgroundJobClient;
        readonly IEmailProviderFactoryHelper _emailProviderFactoryHelper;

        public EmailService(IOptions<SendGridSettings> sendGridSettings, IOptions<SmtpSettings> smtpSettings, IBackgroundJobClient backgroundJobClient, IEmailProviderFactoryHelper emailProviderFactoryHelper)
        {
            _sendGridSettings = sendGridSettings.Value;
            _smtpSettings = smtpSettings.Value;
            _backgroundJobClient = backgroundJobClient;
            _emailProviderFactoryHelper = emailProviderFactoryHelper;
        }

        public void SendEmailConfirmation(ApplicationUser user, string url)
        {
            //Build the Email Message
            var message = new EmailMessage();
            message.To.Add(new EmailAddress{FirstName = user.FirstName, LastName = user.LastName, Address = user.Email});
            message.Substitutions.Add("-FirstName-", user.FirstName);
            message.Substitutions.Add("-LastName-", user.LastName);
            message.Substitutions.Add("-ConfirmationLink-", url);
            message.TemplateId = _sendGridSettings.EmailVerification;
            message.From = new EmailAddress{Address = _smtpSettings.UserName, Name = _smtpSettings.FromName };
            message.Subject = "Email Confirmation";
            IEmailProvider emailProvider = _emailProviderFactoryHelper.GetEmailProvider(EmailProviderType.SendGrid);
            emailProvider.SendMail(message);
        }

        public void SendResetPasswordRequestEmail(ApplicationUser user, string url)
        {
            //Build the Email Message
            var message = new EmailMessage();
            message.To.Add(new EmailAddress{ FirstName = user.FirstName, LastName = user.LastName, Address = user.Email });
            message.Substitutions.Add("-FirstName-", user.FirstName);
            message.Substitutions.Add("-LastName-", user.LastName);
            message.Substitutions.Add("-PasswordResetLink-", url);
            message.TemplateId = _sendGridSettings.PasswordReset;
            message.From = new EmailAddress{ Address = _smtpSettings.UserName, Name = _smtpSettings.FromName };
            message.Subject = "Password Reset";
            IEmailProvider emailProvider = _emailProviderFactoryHelper.GetEmailProvider(EmailProviderType.SendGrid);
            emailProvider.SendMail(message);
        }
    }
}
