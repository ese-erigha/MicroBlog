using System;
using System.IO;
using System.Threading.Tasks;
using MicroBlog.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MicroBlog.Helpers
{

    public abstract class FileWriterHelper : IFileWriterHelper
    {
        public byte[] ConvertFileToBytes(IFormFile file)
        {
            byte[] fileBytes;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return fileBytes; 
        }


        //Template Method Pattern
        public async Task<FileUploadResult> UploadFile(IFormFile file)
        {
            byte[] fileBytes = ConvertFileToBytes(file);

            bool isValidFileFormat = ValidateFileFormat(file);  //Implemented by sub classes

            var uploadResult = new FileUploadResult();

            if(!isValidFileFormat)
            {
                uploadResult.Succeeded = false;
                uploadResult.ErrorMessage = "Invalid File Format!";


            }
            //Implemented by sub classes
            else if (!ValidateFileSize(file)){  

                uploadResult.Succeeded = false;
                uploadResult.ErrorMessage = "File size too large";
            }
            else{
                uploadResult.FileName = await WriteFile(file);
                uploadResult.Succeeded = true;
            }

            return uploadResult;
        }


        public async Task<string> WriteFile(IFormFile file)
        {
            string fileName;
            try
            {
                var fileSplit = file.FileName.Split('.');
                var extension = "." + fileSplit[fileSplit.Length - 1];
                fileName = Guid.NewGuid().ToString() + extension; //Create a new Name 
                                                                  //for the file due to security reasons.
                var path = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return fileName;
        }

        //Implemented by Subclasses
        public abstract bool ValidateFileFormat(IFormFile file);

        public abstract bool ValidateFileSize(IFormFile file);
    }
}
