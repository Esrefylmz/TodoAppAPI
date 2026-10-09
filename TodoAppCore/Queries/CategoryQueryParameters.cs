using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAppCore.Queries
{
    public class CategoryQueryParameters
    {
        public string? Search { get; set; }

        public CategorySortBy? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public enum CategorySortBy
    {
        Id,
        Title
    }


}