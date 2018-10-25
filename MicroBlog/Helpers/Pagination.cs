using System;
using System.Collections.Generic;
using MicroBlog.Models.ViewModels;

namespace MicroBlog.Helpers
{
    public class Pagination<E> where E: class
    {
        public PaginationViewModel Pager {get; set;}

        public IEnumerable<E> PagedItems {get; set;}
    }
}
