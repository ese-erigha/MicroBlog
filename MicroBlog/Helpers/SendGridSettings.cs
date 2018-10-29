using System;
namespace MicroBlog.Helpers
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; }

        public string EmailVerification { get; set; }

        public string PasswordReset { get; set; }
    }
}
