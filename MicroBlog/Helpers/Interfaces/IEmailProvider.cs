using System;
using System.Threading.Tasks;

namespace MicroBlog.Helpers.Interfaces
{
    public interface IEmailProvider
    {
        Task SendMail(EmailMessage message);

    }
}
