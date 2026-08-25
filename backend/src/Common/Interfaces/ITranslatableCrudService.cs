using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Common.Interfaces
{
    /// <summary>
    /// What every translated content module can do beyond plain CRUD.
    ///
    /// Declared once so the controller base can offer reordering and the moved-slug
    /// lookup without each module re-announcing them — the same bargain
    /// <see cref="IBaseCrudService{TEntity, TDTO, TParam, TPaginationDTO, TFilterParam}"/>
    /// already makes for the five ordinary endpoints.
    /// </summary>
    public interface ITranslatableCrudService<TEntity, TDTO, TParam, TPaginationDTO, TFilterParam>
        : IBaseCrudService<TEntity, TDTO, TParam, TPaginationDTO, TFilterParam>
        where TEntity : class, IBaseEntity
        where TDTO : class
        where TParam : class
        where TPaginationDTO : class
        where TFilterParam : BasePaginationParam
    {
        /// <summary>Applies a new display order in one write.</summary>
        Task ReorderAsync(ReorderParam param);

        /// <summary>The address a retired slug now points to, for the 301.</summary>
        Task<string?> ResolveMovedSlugAsync(string localeCode, string oldSlug);
    }
}
