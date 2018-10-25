using System;
using System.Threading.Tasks;
using MicroBlog.Helpers;
using Microsoft.AspNetCore.Http;

namespace MicroBlog.Services.Interfaces
{
    public interface IFileHandlerService : IService
    {
        Task<FileUploadResult> UploadFile(IFormFile file);
    }
}
