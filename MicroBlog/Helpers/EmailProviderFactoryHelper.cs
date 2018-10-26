using System;
using MicroBlog.Helpers.Interfaces;
using Microsoft.Extensions.Options;

namespace MicroBlog.Helpers
{
	public class EmailProviderFactoryHelper : IEmailProviderFactoryHelper
    {

        readonly EmailConfiguration _emailConfiguration;

        public EmailProviderFactoryHelper(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }

        public IEmailProvider GetEmailProvider(EmailProviderType providerType)
        {
            switch(providerType)
            {
                case EmailProviderType.SendGrid:
                    return new SendGridEmailProvider(_emailConfiguration.SendGrid.ApiKey);

                default:
                    return new DefaultEmailProvider();
            }
        }
    }
}
