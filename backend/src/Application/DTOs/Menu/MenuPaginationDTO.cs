using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.Menu
{
    public class MenuPaginationDTO : BasePaginationItemDTO
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
    }
}
