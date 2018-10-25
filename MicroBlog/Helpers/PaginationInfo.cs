using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroBlog.Helpers
{
    public class PaginationInfo
    {

        const int maxPageSize = 2;

        public int PageNumber { get; set; } = 1;

        int _pageSize = 2;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set 
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        int _channelType = (int)Channels.ChannelType.WEB;

        public int Channel { 

            get{


                return _channelType;
            } 

            set{
                _channelType = value;
            } 
        
        }

    }
}
