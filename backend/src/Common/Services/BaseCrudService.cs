using System.Linq.Expressions;
using System.Reflection;
using Mapster;
using LeksanaStudio.Common.Constants;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.Common.Services
{
    public abstract class BaseCrudService<TEntity, TDTO, TParam, TPaginationDTO, TFilterParam>
        : IBaseCrudService<TEntity, TDTO, TParam, TPaginationDTO, TFilterParam>
        where TEntity : class, IBaseEntity
        where TDTO : class
        where TParam : class
        where TPaginationDTO : class
        where TFilterParam : BasePaginationParam
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly ICurrentUserService _currentUserService;

        protected BaseCrudService(
            IBaseRepository<TEntity> repository,
            ICurrentUserService currentUserService
        )
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public virtual async Task<Guid> CreateAsync(TParam param)
        {
            return await _repository.ExecuteInTransactionAsync(async () =>
            {
                var entity = param.Adapt<TEntity>();

                entity.CreatedDate = DateTimeOffset.UtcNow;
                entity.CreatedBy = _currentUserService.UserName ?? AuditConstants.SystemUser;

                OnCreating(entity, param);

                var result = await _repository.CreateAsync(entity);
                return result.Id;
            });
        }

        public virtual async Task<Guid> UpdateAsync(Guid id, TParam param)
        {
            return await _repository.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _repository.GetAsync(q => q.Where(e => e.Id == id));
                if (entity == null)
                    throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found");

                param.Adapt(entity);
                entity.UpdatedDate = DateTimeOffset.UtcNow;
                entity.UpdatedBy = _currentUserService.UserName ?? AuditConstants.SystemUser;

                OnUpdating(entity, param);

                await _repository.UpdateAsync(entity);
                return entity.Id;
            });
        }

        public virtual async Task<Guid> DeleteAsync(Guid id)
        {
            return await _repository.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _repository.GetAsync(q => q.Where(e => e.Id == id));
                if (entity == null)
                    throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found");

                entity.IsDeleted = true;
                entity.DeletedDate = DateTimeOffset.UtcNow;
                entity.DeletedBy = _currentUserService.UserName ?? AuditConstants.SystemUser;

                await _repository.UpdateAsync(entity);
                return entity.Id;
            });
        }

        public virtual async Task<TDTO> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(q => q.Where(e => e.Id == id));
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found");

            var dto = entity.Adapt<TDTO>();
            await EnrichSingleAsync(entity, dto);
            return dto;
        }

        public virtual async Task<IEnumerable<TPaginationDTO>> GetListAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? query = null
        )
        {
            var entities = await _repository.GetListAsync(query);
            return entities.Adapt<IEnumerable<TPaginationDTO>>();
        }

        public virtual async Task<PaginationResponse<TPaginationDTO>> GetPaginationAsync(
            TFilterParam param
        )
        {
            var (sortExpr, descending) = GetSortExpression(param);
            var (entities, totalCount) = await _repository.GetPaginationAsync(
                param.Page,
                param.PageSize,
                q => ApplyFilter(q, param),
                sortExpr,
                descending
            );

            var items = entities.Adapt<List<TPaginationDTO>>();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] is BasePaginationItemDTO paginationItem)
                    paginationItem.Number = ((param.Page - 1) * param.PageSize) + i + 1;
            }

            await EnrichPaginationAsync(items);

            return new PaginationResponse<TPaginationDTO>
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / param.PageSize),
                Page = param.Page,
                PageSize = param.PageSize,
            };
        }

        // Same filter + sort as the table, but every matching row (no paging) —
        // used by exports that must ignore pagination but honour search/filter/sort.
        public virtual async Task<List<TPaginationDTO>> GetExportAsync(TFilterParam param)
        {
            var (sortExpr, descending) = GetSortExpression(param);
            var (entities, _) = await _repository.GetPaginationAsync(
                1,
                int.MaxValue,
                q => ApplyFilter(q, param),
                sortExpr,
                descending
            );

            var items = entities.Adapt<List<TPaginationDTO>>();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] is BasePaginationItemDTO paginationItem)
                    paginationItem.Number = i + 1;
            }

            await EnrichPaginationAsync(items);

            return items;
        }

        /// <summary>
        /// Last chance to adjust a new entity before it is written. Runs inside the
        /// transaction, after the request has been mapped and the audit fields stamped.
        /// </summary>
        /// <remarks>
        /// Use for values that must never be persisted verbatim from a request — a
        /// password that has to be hashed, a server-assigned default, a computed field.
        /// </remarks>
        protected virtual void OnCreating(TEntity entity, TParam param) { }

        /// <summary>
        /// Same as <see cref="OnCreating"/>, for updates. The entity still holds its
        /// stored values for any property the request does not map, so this is also
        /// where "leave unchanged when the request omits it" is expressed.
        /// </summary>
        protected virtual void OnUpdating(TEntity entity, TParam param) { }

        protected virtual Task EnrichPaginationAsync(List<TPaginationDTO> items) =>
            Task.CompletedTask;

        protected virtual Task EnrichSingleAsync(TEntity entity, TDTO dto) => Task.CompletedTask;

        protected virtual IQueryable<TEntity> ApplyFilter(
            IQueryable<TEntity> query,
            TFilterParam param
        ) => query;

        protected virtual (
            Expression<Func<TEntity, object?>>? orderBy,
            bool descending
        ) GetSortExpression(TFilterParam param)
        {
            if (string.IsNullOrWhiteSpace(param.SortBy))
                return (null, false);

            var property = typeof(TEntity).GetProperty(
                param.SortBy,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
            );

            if (property == null)
                return (null, false);

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var propertyAccess = Expression.Property(parameter, property);
            var converted = Expression.Convert(propertyAccess, typeof(object));
            var keySelector = Expression.Lambda<Func<TEntity, object?>>(converted, parameter);

            return (
                keySelector,
                param.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
