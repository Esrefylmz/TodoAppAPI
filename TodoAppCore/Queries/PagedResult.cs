using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAppCore.Queries
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
    }
}