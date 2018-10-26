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

namespace MicroBlog.Services.Implementations
{
    public class EmailService : IEmailService
    {
        readonly EmailConfiguration _emailConfiguration;
        readonly IBackgroundJobClient _backgroundJobClient;
        readonly IEmailProviderFactoryHelper _emailProviderFactoryHelper;

        public EmailService(IOptions<EmailConfiguration> emailConfiguration, IBackgroundJobClient backgroundJobClient, IEmailProviderFactoryHelper emailProviderFactoryHelper)
        {
            _emailConfiguration = emailConfiguration.Value;
            _backgroundJobClient = backgroundJobClient;
            _emailProviderFactoryHelper = emailProviderFactoryHelper;
        }

        public void SendEmailConfirmation(ApplicationUser user, string url)
        {
            //Build the Email Message
            var message = new EmailMessage();
            message.To.Add(new Helpers.EmailAddress(){FirstName = user.FirstName, LastName = user.LastName, Address = user.Email});
            message.Substitutions.Add("-FirstName-", user.FirstName);
            message.Substitutions.Add("-LastName-", user.LastName);
            message.Substitutions.Add("-ConfirmationLink-", url);
            message.TemplateId = _emailConfiguration.SendGrid.EmailVerification;
            message.From = new Helpers.EmailAddress() {Address = _emailConfiguration.SmtpConfig.Username, Name = _emailConfiguration.SmtpConfig.FromName };
            IEmailProvider emailProvider = _emailProviderFactoryHelper.GetEmailProvider(EmailProviderType.SendGrid);
            var jobId = _backgroundJobClient.Enqueue(() => emailProvider.SendMail(message));
        }

        public void SendResetPasswordRequestEmail(ApplicationUser user, string url)
        {
            //Build the Email Message
            var message = new EmailMessage();
            message.To.Add(new Helpers.EmailAddress() { FirstName = user.FirstName, LastName = user.LastName, Address = user.Email });
            message.Substitutions.Add("-FirstName-", user.FirstName);
            message.Substitutions.Add("-LastName-", user.LastName);
            message.Substitutions.Add("-PasswordResetLink-", url);
            message.TemplateId = _emailConfiguration.SendGrid.PasswordReset;
            message.From = new Helpers.EmailAddress() { Address = _emailConfiguration.SmtpConfig.Username, Name = _emailConfiguration.SmtpConfig.FromName };
            IEmailProvider emailProvider = _emailProviderFactoryHelper.GetEmailProvider(EmailProviderType.SendGrid);
            var jobId = _backgroundJobClient.Enqueue(() => emailProvider.SendMail(message));
        }
    }
}
