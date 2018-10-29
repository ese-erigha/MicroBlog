using System;
using Hangfire;
using MicroBlog.Helpers.Interfaces;
using Microsoft.Extensions.Options;

namespace MicroBlog.Helpers
{
	public class EmailProviderFactoryHelper : IEmailProviderFactoryHelper
    {
        readonly IBackgroundJobClient _backgroundJobClient;
        readonly IOptions<SendGridSettings> _sendGridSettings;
        public IEmailProvider _emailProvider;

        public EmailProviderFactoryHelper(IBackgroundJobClient backgroundJobClient, IOptions<SendGridSettings> sendGridSettings)
        {
            _backgroundJobClient = backgroundJobClient;
            _sendGridSettings = sendGridSettings;
        }

        public IEmailProvider GetEmailProvider(EmailProviderType providerType)
        {
            switch(providerType)
            {
                case EmailProviderType.SendGrid:
                    _emailProvider = new SendGridEmailProvider(_backgroundJobClient,_sendGridSettings);
                    break;

                default:
                    _emailProvider = new DefaultEmailProvider();
                    break;
            }
            return _emailProvider;
        }
    }
}
