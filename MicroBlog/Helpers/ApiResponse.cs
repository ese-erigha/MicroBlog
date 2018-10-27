using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Helpers
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }

        public string Message { get; set; }
    }



}
