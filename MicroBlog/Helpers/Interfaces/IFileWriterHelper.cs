using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MicroBlog.Helpers.Interfaces
{
    public interface IFileWriterHelper
    {
        Task<FileUploadResult> UploadFile(IFormFile file);

        Task<string> WriteFile(IFormFile file);

        byte[] ConvertFileToBytes(IFormFile file);

    }
}
