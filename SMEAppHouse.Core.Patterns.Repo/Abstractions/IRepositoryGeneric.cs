using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace SMEAppHouse.Core.Patterns.Repo.Abstractions
{
    public interface IRepositoryGeneric<TEntity, TPk> : IDisposable
        where TEntity : class
        where TPk : struct
    {
        DbContext DbContext { get; set; }
        DbSet<TEntity> DbSet { get; set; }

        IQueryable<TEntity> Query(string sql, params object[] parameters);

        Task<TEntity> FindAsync(params object[] keyValues);
        //Task<TEntity> Find(Expression<Func<TEntity, object>> includeSelector, params object[] keyValues);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        //Task<TEntity> Search(CancellationToken cancelToken
        //                                    , params object[] keyValues);
        //Task<TEntity> Search(CancellationToken cancelToken
        //                                    , bool noTracking = true
        //                                    , params object[] keyValues);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        Task<IEnumerable<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int fetchLimit = 0, bool disableTracking = true);

        Task AddAsync(TEntity entity);
        Task AddAsync(params TEntity[] entities);
        Task AddAsync(IEnumerable<TEntity> entities);


        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(object id);
        Task DeleteAsync(params TEntity[] entities);
        Task DeleteAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);


        //void Update(TEntity entity);
        Task UpdateAsync(params TEntity[] entities);
        Task UpdateAsync(IEnumerable<TEntity> entities);

        Task CommitAsync();
    }
}
