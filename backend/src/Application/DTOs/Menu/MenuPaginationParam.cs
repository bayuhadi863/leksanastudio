using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.Menu
{
    public class MenuPaginationParam : BasePaginationParam
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
