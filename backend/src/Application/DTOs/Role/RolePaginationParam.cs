using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.Role
{
    public class RolePaginationParam : BasePaginationParam
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
