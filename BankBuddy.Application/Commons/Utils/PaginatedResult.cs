using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Commons.Utils
{
    public class PaginatedResult<T>(IEnumerable<T> items, int page, int pageSize, int totalItems)
    {
        public IEnumerable<T> Items { get; set; } = items;
        public int Page { get; set; } = page;
        public int PageSize { get; set; } = pageSize;
        public int TotalItems { get; set; } = totalItems;
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
