using System;
namespace MicroBlog.Helpers
{
    public class SendGridConfig
    {
        public string ApiKey { get; set; }

        public string EmailVerification { get; set; }

        public string PasswordReset { get; set; }
    }
}
