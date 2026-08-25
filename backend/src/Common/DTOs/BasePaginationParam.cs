using System.ComponentModel.DataAnnotations;

namespace LeksanaStudio.Common.DTOs
{
    public class BasePaginationParam
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }

        public string SortOrder { get; set; } = "asc";

        public string? Search { get; set; }
    }
}
