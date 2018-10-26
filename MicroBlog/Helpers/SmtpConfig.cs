using System;
namespace MicroBlog.Helpers
{
    public class SmtpConfig
    {
        public string Server { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FromName { get; set; }
    }
}
