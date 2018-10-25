using System;
using MicroBlog.Helpers.Interfaces;

namespace MicroBlog.Helpers
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public SmtpConfig SmtpConfig { get; set; }

        public SendGridConfig SendGrid { get; set; }
    }
}
