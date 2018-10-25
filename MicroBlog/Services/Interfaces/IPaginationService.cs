using System;
using System.Linq;
using MicroBlog.Helpers;

namespace MicroBlog.Services.Interfaces
{
    public interface IPaginationService : IService
    {
        Pagination<E> Paginate<E>(IQueryable<E> source, PaginationInfo paginationInfo) where E : class;
    }
}
