using LeksanaStudio.Application.DTOs.Menu;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IMenuService
        : IBaseCrudService<Menu, MenuDTO, MenuParam, MenuPaginationDTO, MenuPaginationParam>
    {
        Task<IEnumerable<MenuDTO>> GetUserAccessibleMenusAsync();
    }
}
