using System;
using System.Threading.Tasks;
using MicroBlog.Helpers;

namespace MicroBlog.Services.Interfaces
{
    public interface IEmailService : IService
    {
        void SendEmailAsync(EmailMessage message);
    }
}
