using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.Common.Interfaces
{
    public interface IBaseCrudService<TEntity, TDTO, TParam, TPaginationDTO, TFilterParam>
        where TEntity : class, IBaseEntity
        where TDTO : class
        where TParam : class
        where TPaginationDTO : class
        where TFilterParam : BasePaginationParam
    {
        Task<Guid> CreateAsync(TParam param);
        Task<Guid> UpdateAsync(Guid id, TParam param);
        Task<Guid> DeleteAsync(Guid id);
        Task<TDTO> GetAsync(Guid id);
        Task<IEnumerable<TPaginationDTO>> GetListAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? query = null);
        Task<PaginationResponse<TPaginationDTO>> GetPaginationAsync(TFilterParam param);
        Task<List<TPaginationDTO>> GetExportAsync(TFilterParam param);
    }
}
