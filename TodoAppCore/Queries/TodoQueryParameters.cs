using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAppCore.Queries
{
    public class TodoQueryParameters
    {
        public bool? IsCompleted { get; set; }
        public int? CategoryId { get; set; }
        public string? Search { get; set; }

        public TodoSortBy? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public enum TodoSortBy
    {
        Id,
        Title,
        CreatedAt
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }
}
