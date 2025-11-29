using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using SMEAppHouse.Core.Patterns.Repo.Generic.Paging;

namespace SMEAppHouse.Core.Patterns.Repo.Generic
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        protected readonly DbContext DbContext;
        protected readonly DbSet<T> DbSet;

        public RepositoryAsync(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            IQueryable<T> query = DbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).FirstOrDefaultAsync();
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool disableTracking = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<T> query = DbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken);
            return await query.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public async Task<EntityEntry<T>> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddAsync(params T[] entities)
        {
            await DbSet.AddRangeAsync(entities);
        }


        public async Task AddAsync(IEnumerable<T> entities,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await DbSet .AddRangeAsync(entities, cancellationToken);
        }


        [Obsolete("Use get list ")]
        public IEnumerable<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public EntityEntry<T> UpdateAsync(T entity)
        {
            return DbSet.Update(entity);
        }

        public async Task<EntityEntry<T>> AddAsync(T entity)
        {
            return await AddAsync(entity, new CancellationToken());
        }
    }
}