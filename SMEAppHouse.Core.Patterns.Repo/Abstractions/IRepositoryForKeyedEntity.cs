using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Paging;
using System.Linq.Expressions;

namespace SMEAppHouse.Core.Patterns.Repo.Abstractions;

/// <summary>
/// Defines the repository operations for entities with a primary key.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TPk">The type of the primary key of the entity.</typeparam>
public interface IRepositoryForKeyedEntity<TEntity, TPk> : IDisposable
    where TEntity : class, IKeyedEntity<TPk>
    where TPk : struct
{
    DbContext DbContext { get; set; }
    DbSet<TEntity> DbSet { get; set; }

    // Querying methods
    IQueryable<TEntity> Query(string sql, params object[] parameters);
    Task<TEntity?> FindAsync(params object[] keyValues);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, object>> includeSelector, params object[] keyValues);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                   bool disableTracking = true);

    Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter);
    Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null,
                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                            bool disableTracking = true);

    Task<PagedResult<TEntity, TPk>> GetListAsync(PageRequest pageRequest, Expression<Func<TEntity, bool>> filter);
    Task<PagedResult<TEntity, TPk>> GetListAsync(PageRequest pageRequest,
                                                 Expression<Func<TEntity, bool>>? filter = null,
                                                 Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                 bool disableTracking = true);

    // CRUD methods
    Task AddAsync(TEntity entity);
    Task AddAsync(params TEntity[] entities);
    Task AddAsync(IEnumerable<TEntity> entities);

    Task DeleteAsync(TEntity entity);
    Task DeleteAsync(TPk id);
    Task DeleteAsync(params TEntity[] entities);
    Task DeleteAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    Task UpdateAsync(TEntity entity);
    Task UpdateAsync(params TEntity[] entities);
    Task UpdateAsync(IEnumerable<TEntity> entities);

    Task CommitAsync();

    // Dispose method
    void Dispose();
}
