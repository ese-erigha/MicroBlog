using System;
using System.Threading.Tasks;
using Hangfire;
using MicroBlog.Helpers;
using MicroBlog.Services.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MicroBlog.Services.Implementations
{
    public class EmailService : IEmailService
    {
        readonly EmailConfiguration _emailConfiguration;
        readonly IBackgroundJobClient _backgroundJobClient;

        public EmailService(IOptions<EmailConfiguration> emailConfiguration, IBackgroundJobClient backgroundJobClient)
        {
            _emailConfiguration = emailConfiguration.Value;
            _backgroundJobClient = backgroundJobClient;
        }

        public void SendEmailAsync(EmailMessage emailMessage)
        {
            var mailMessage = BuildEmail(emailMessage);

            var status = _backgroundJobClient.Enqueue(() => Execute(mailMessage));

        }

        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Execute(MimeMessage message)
        {

        }

        public MimeMessage BuildEmail(EmailMessage emailMessage)
        {
            return new MimeMessage();
        }
    }
}
