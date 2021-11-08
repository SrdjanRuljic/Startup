using System.Collections.Generic;

namespace Application.Common.Pagination.Models
{
    public class PaginationResultViewModel<T>
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public List<T> List { get; set; }
    }
}
