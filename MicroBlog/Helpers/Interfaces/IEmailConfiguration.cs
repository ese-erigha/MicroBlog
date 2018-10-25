using System;
namespace MicroBlog.Helpers.Interfaces
{
    public interface IEmailConfiguration
    {
        SmtpConfig SmtpConfig { get; set; }

        SendGridConfig SendGrid { get; set; }
    }
}
