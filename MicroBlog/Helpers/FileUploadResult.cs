using System;
namespace MicroBlog.Helpers
{
    public class FileUploadResult
    {
        public bool Succeeded { get; set; }

        public string ErrorMessage { get; set; }

        public string FileName { get; set; }
    }
}
