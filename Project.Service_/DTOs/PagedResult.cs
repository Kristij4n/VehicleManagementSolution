using System.Collections.Generic;

namespace Project.Service_.DTOs
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
} //paging result returns total count