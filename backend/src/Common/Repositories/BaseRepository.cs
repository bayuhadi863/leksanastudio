using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Common.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : class, IBaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetAsync(Func<IQueryable<T>, IQueryable<T>> query)
        {
            return await query(_dbSet.AsQueryable()).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(
            Func<IQueryable<T>, IQueryable<T>>? query = null
        )
        {
            var baseQuery = _dbSet.AsQueryable();
            return query == null
                ? await baseQuery.ToListAsync()
                : await query(baseQuery).ToListAsync();
        }

        public virtual async Task<(IEnumerable<T> Items, long TotalCount)> GetPaginationAsync(
            int page,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>>? filter = null,
            Expression<Func<T, object?>>? orderBy = null,
            bool descending = false
        )
        {
            var baseQuery = _dbSet.AsQueryable();

            if (filter != null)
                baseQuery = filter(baseQuery);

            var totalCount = await baseQuery.LongCountAsync();

            IOrderedQueryable<T> orderedQuery;
            if (orderBy != null)
            {
                var nullCheck = BuildNullCheckExpression(orderBy);
                orderedQuery = descending
                    ? baseQuery.OrderBy(nullCheck).ThenByDescending(orderBy)
                    : baseQuery.OrderBy(nullCheck).ThenBy(orderBy);
            }
            else
            {
                orderedQuery = baseQuery.OrderByDescending(e => e.CreatedDate);
            }

            var items = await orderedQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // Builds a bool expression that is true when the key is null, enabling NULLS LAST ordering.
        private static Expression<Func<T, bool>> BuildNullCheckExpression(
            Expression<Func<T, object?>> keySelector
        )
        {
            var param = keySelector.Parameters[0];
            var body = keySelector.Body is UnaryExpression { NodeType: ExpressionType.Convert } u
                ? u.Operand
                : keySelector.Body;

            // Non-nullable value types can never be null — short-circuit to constant false.
            if (body.Type.IsValueType && Nullable.GetUnderlyingType(body.Type) == null)
                return Expression.Lambda<Func<T, bool>>(Expression.Constant(false), param);

            return Expression.Lambda<Func<T, bool>>(
                Expression.Equal(body, Expression.Constant(null, body.Type)),
                param
            );
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<int> CountAsync(Func<IQueryable<T>, IQueryable<T>>? query = null)
        {
            var baseQuery = _dbSet.AsQueryable();
            return query == null
                ? await baseQuery.CountAsync()
                : await query(baseQuery).CountAsync();
        }

        public virtual async Task<long> LongCountAsync(
            Func<IQueryable<T>, IQueryable<T>>? query = null
        )
        {
            var baseQuery = _dbSet.AsQueryable();
            return query == null
                ? await baseQuery.LongCountAsync()
                : await query(baseQuery).LongCountAsync();
        }

        public virtual async Task<List<TResult>> GetAggregateAsync<TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> query
        )
        {
            return await query(_dbSet.AsQueryable()).ToListAsync();
        }

        public virtual async Task BeginTransactionAsync() =>
            await _context.Database.BeginTransactionAsync();

        public virtual async Task CommitTransactionAsync() =>
            await _context.Database.CommitTransactionAsync();

        public virtual async Task RollbackTransactionAsync() =>
            await _context.Database.RollbackTransactionAsync();

        public virtual async Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<Task<TResult>> action
        )
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await action();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
