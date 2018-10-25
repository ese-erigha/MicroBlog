using System;
namespace MicroBlog.Models.ViewModels
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public int StartPage { get; private set; }

        public int EndPage { get; private set; }

        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }


        public PaginationViewModel(int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (count > 0) ? (int)Math.Ceiling(count / (double)pageSize) : 0;

            StartPage = 0;
            EndPage = 0;

            if (TotalPages <= PageSize)
            {
                StartPage = 1;
                EndPage = TotalPages;
            }
            else
            {
                if (CurrentPage <= 6)
                {
                    StartPage = 1;
                    EndPage = 10;
                }
                else if (CurrentPage + 4 >= TotalPages)
                {
                    StartPage = TotalPages - 9;
                    EndPage = TotalPages;
                }
                else
                {
                    StartPage = CurrentPage - 5;
                    EndPage = CurrentPage + 4;
                }
            }

        }
    }
}
