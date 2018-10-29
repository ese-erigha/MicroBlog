using System;
using System.Threading.Tasks;
using MicroBlog.Helpers.Interfaces;

namespace MicroBlog.Helpers
{
    public class DefaultEmailProvider : IEmailProvider
    {
        public void SendMail(EmailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
