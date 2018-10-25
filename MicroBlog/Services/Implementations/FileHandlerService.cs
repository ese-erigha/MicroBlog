using System;
using System.Threading.Tasks;
using MicroBlog.Helpers;
using MicroBlog.Helpers.Interfaces;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MicroBlog.Services.Implementations
{
    public class FileHandlerService : IFileHandlerService
    {
        readonly IImageWriterHelper _imageWriterHelper;

        public FileHandlerService(IImageWriterHelper imageWriterHelper)
        {
            _imageWriterHelper = imageWriterHelper;
        }

        public async Task<FileUploadResult> UploadFile(IFormFile file)
        {
            FileUploadResult uploadResult =  await _imageWriterHelper.UploadFile(file);
            return uploadResult;
        }
    }
}
