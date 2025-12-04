using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared
{
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Search { get; set; }
        public SortingEnum? Sort { get; set; }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = (value < 0) ? 1 : value; }
        }

        private int _pageSize = 5;

        public int PageSize
        {
            get { return _pageSize; }
            set 
            { 
                if (value <= 0)
                     _pageSize = 5; 
                else if (value > 10)
                     _pageSize = 10;
                else
                     _pageSize = value;
            }
        }

    }
}
