using System;
namespace MicroBlog.Helpers.Interfaces
{
    public interface IEmailProviderFactoryHelper
    {
        IEmailProvider GetEmailProvider(EmailProviderType providerType);
    }
}
