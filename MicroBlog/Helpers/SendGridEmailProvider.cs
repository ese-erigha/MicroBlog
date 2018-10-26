using System;
using System.Threading.Tasks;
using Hangfire;
using MicroBlog.Helpers.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MicroBlog.Helpers
{
    public class SendGridEmailProvider : IEmailProvider
    {
        readonly string _apiKey;

        public SendGridEmailProvider(string apiKey)
        {
            _apiKey = apiKey;
        }

        [AutomaticRetry(Attempts = 20,OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task SendMail(EmailMessage message)
        {
            var msg = new SendGridMessage();

            message.To.ForEach(address => msg.AddTo(new SendGrid.Helpers.Mail.EmailAddress(address.Address,address.Name)));
            foreach(var item in message.Substitutions)
            {
                msg.AddSubstitution(item.Key, item.Value);
            }
            msg.SetTemplateId(message.TemplateId);
            message.Bcc.ForEach(bcc => msg.AddBcc(bcc.Address));
            message.CC.ForEach(cc => msg.AddCc(cc.Address));
            message.Subject = msg.Subject;

            var client = new SendGridClient(_apiKey);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
