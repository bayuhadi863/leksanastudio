using System.Linq.Expressions;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Common.Interfaces
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        Task<T?> GetAsync(Func<IQueryable<T>, IQueryable<T>> query);
        Task<IEnumerable<T>> GetListAsync(Func<IQueryable<T>, IQueryable<T>>? query = null);
        Task<(IEnumerable<T> Items, long TotalCount)> GetPaginationAsync(
            int page,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>>? filter = null,
            Expression<Func<T, object?>>? orderBy = null,
            bool descending = false
        );
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities);
        Task<int> CountAsync(Func<IQueryable<T>, IQueryable<T>>? query = null);
        Task<long> LongCountAsync(Func<IQueryable<T>, IQueryable<T>>? query = null);

        // Run an arbitrary projection (GroupBy/Select/...) over the non-deleted set,
        // materialising the shaped result — used for aggregate/statistics queries.
        Task<List<TResult>> GetAggregateAsync<TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> query
        );

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // Runs <paramref name="action"/> inside a transaction: commits on success,
        // rolls back and rethrows on any exception.
        Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action);
    }
}
