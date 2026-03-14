using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Common
{
    public class PagedResult<T> where T : class
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; } // tong so record trong db
        public int Page {  get; set; } // trang hien tai
        public int PageSize { get; set; } // so record moi trang
        public int TotalPages 
        {
            // tong so trang => totalCount / PageSize lam tron len
            get
            {
                return (int)Math.Ceiling((double)TotalCount / PageSize);
            }
        }
        public bool HasPrevious
        {
            get
            {
                // so sanh neu page hien tai lon hon 1 enable button prev
                return Page > 1;
            }
        }
        public bool HasNext
        {
            get
            {
                // so sanh neu page hien tai dang nho hon tong trang enable button next
                return Page < TotalPages;
            }
        }

    }
}
