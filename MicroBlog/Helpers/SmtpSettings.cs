using System;
namespace MicroBlog.Helpers
{
    public class SmtpSettings
    {
        public string Server { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FromName { get; set; }
    }
}
