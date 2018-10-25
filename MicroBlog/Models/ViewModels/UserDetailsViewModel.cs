using System;
using System.Collections.Generic;

namespace MicroBlog.Models.ViewModels
{
	public class UserDetailsViewModel
    {
        public IEnumerable<UserInfoViewModel> PagedUsers { get; set; }

        public PaginationViewModel Pager { get; set; }
    }
}
