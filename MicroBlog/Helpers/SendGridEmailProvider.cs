using System;
using System.Threading.Tasks;
using Hangfire;
using MicroBlog.Helpers.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MicroBlog.Helpers
{
    public class SendGridEmailProvider : IEmailProvider
    {
        readonly IBackgroundJobClient _backgroundJobClient;
        readonly SendGridSettings _sendGridSettings;

        public SendGridEmailProvider(IBackgroundJobClient backgroundJobClient, IOptions<SendGridSettings> sendGridSettings)
        {
            _backgroundJobClient = backgroundJobClient;
            _sendGridSettings = sendGridSettings.Value;
        }


        public void SendMail(EmailMessage message)
        {
            var sendGridMessage = new SendGridMessage();

            message.To.ForEach(address => sendGridMessage.AddTo(new SendGrid.Helpers.Mail.EmailAddress(address.Address,address.Name)));
            foreach(var item in message.Substitutions)
            {
                sendGridMessage.AddSubstitution(item.Key, item.Value);
            }

            sendGridMessage.SetTemplateId(message.TemplateId);
            message.Bcc.ForEach(bcc => sendGridMessage.AddBcc(bcc.Address));
            message.CC.ForEach(cc => sendGridMessage.AddCc(cc.Address));
            sendGridMessage.Subject = message.Subject;
            sendGridMessage.SetFrom(new SendGrid.Helpers.Mail.EmailAddress(message.From.Address,message.From.Name));

            string apiKey = _sendGridSettings.ApiKey;

            var jobId = _backgroundJobClient.Enqueue(() => Send(sendGridMessage, apiKey));
        }

        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task Send(SendGridMessage message, string apiKey)
        {
            try{

                var client = new SendGridClient(apiKey);
                var response = await client.SendEmailAsync(message).ConfigureAwait(false); 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
