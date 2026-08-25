using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.User
{
    public class UserPaginationParam : BasePaginationParam
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
