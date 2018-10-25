using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MicroBlog.Helpers;
using MicroBlog.Models.ViewModels;
using MicroBlog.Services.Interfaces;

namespace MicroBlog.Services.Implementations
{
    public class PaginationService : IPaginationService
    {
        readonly IMapper _mapper;
        public PaginationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Pagination<E> Paginate<E>(IQueryable<E> source, PaginationInfo paginationInfo) where E : class
        {
            var count = source.Count();
            IEnumerable<E> pagedItems = new List<E>();

            if (count > 0)
            {
                pagedItems = source.Skip((paginationInfo.PageNumber - 1) * paginationInfo.PageSize)
                              .Take(paginationInfo.PageSize)
                              .AsEnumerable();
            }

            var pager = new PaginationViewModel(count, paginationInfo.PageNumber, paginationInfo.PageSize);
            return new Pagination<E>() { Pager = pager, PagedItems = pagedItems };

        }
    }
}
