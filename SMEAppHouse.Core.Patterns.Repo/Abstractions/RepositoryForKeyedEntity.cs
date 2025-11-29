using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SMEAppHouse.Core.CodeKits.Helpers;
using SMEAppHouse.Core.CodeKits.Tools;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Paging;
using System.Linq.Expressions;
using System.Reflection;

namespace SMEAppHouse.Core.Patterns.Repo.Abstractions;

public class RepositoryForKeyedEntity<TEntity, TPk> : IRepositoryForKeyedEntity<TEntity, TPk>
    where TEntity : class, IKeyedEntity<TPk>
    where TPk : struct
{
    public DbContext DbContext { get; set; }
    public DbSet<TEntity> DbSet { get; set; }

    public RepositoryForKeyedEntity(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentException(null, nameof(dbContext));
        DbSet = DbContext.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> Query(string sql, params object[] parameters) => DbSet.FromSqlRaw(sql, parameters);

    public async Task<TEntity?> FindAsync(params object[] keyValues) => await DbSet.FindAsync(keyValues);
    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, object>> includeSelector,
                                                        params object[] keyValues)
    {
        return await DbSet
            .Include(includeSelector)
            .FirstOrDefaultAsync(p => keyValues.Contains(p.Id));
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbContext.Set<TEntity>().AnyAsync(predicate);
    }

    public async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>>? predicate = null
        , Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
        , bool disableTracking = true)
    {
        IQueryable<TEntity> query = DbSet;
        if (disableTracking) query = query.AsNoTracking();
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        return await query.FirstOrDefaultAsync();
    }

    /*public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }*/

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await GetListAsync(filter, null, null);
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(
                Expression<Func<TEntity, bool>>? filter = null,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                bool disableTracking = true)
    {
        try
        {
            var result = await RetryCodeKit.Do(async () =>
            {
                IQueryable<TEntity> query = DbSet;

                if (disableTracking) query = query.AsNoTracking();
                if (filter != null) query = query.Where(filter);
                if (include != null) query = include(query);

                var qryable = orderBy?.Invoke(query) ?? query;
                var results = await query.ToListAsync();
                return results.OrderBy(x => Guid.NewGuid());
            }, new TimeSpan(0, 0, 0, 10));
            return result;
        }
        catch (Exception ex)
        {
            throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
        }
    }


    /*public async Task<PagedResult<TEntity, TPk>> FindAsync(Expression<Func<TEntity, bool>> predicate, PagingRequest pagingRequest)
    {
        // Get the total count of the items matching the predicate
        var totalCount = await DbContext.Set<TEntity>().Where(predicate).CountAsync();

        // Calculate the total number of pages
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagingRequest.PageSize);

        // Get the paged data (skipping and taking based on PageNumber and PageSize)
        var items = await DbContext.Set<TEntity>()
            .Where(predicate)
            .Skip((pagingRequest.PageNumber - 1) * pagingRequest.PageSize) // Skip the items for previous pages
            .Take(pagingRequest.PageSize) // Take the items for the current page
            .ToListAsync();

        return new PagedResult<TEntity, TPk>
        {
            Items = items,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }*/

    public async Task<PagedResult<TEntity, TPk>> GetListAsync(PageRequest pagingRequest, Expression<Func<TEntity, bool>> filter)
    {
        return await GetListAsync(pagingRequest, filter, null, null);
    }

    public async Task<PagedResult<TEntity, TPk>> GetListAsync(PageRequest pageRequest,
                                                                Expression<Func<TEntity, bool>>? filter = null,
                                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                bool disableTracking = true)
    {
        try
        {
            (int TotalRecords, int TotalPages, List<TEntity> Items) result = await RetryCodeKit.Do(async () =>
            {
                IQueryable<TEntity> query = DbSet;

                if (disableTracking) query = query.AsNoTracking();
                if (filter != null) query = query.Where(filter);
                if (include != null) query = include(query);

                var qryable = orderBy?.Invoke(query) ?? query;

                // Get the total count of the filtered items
                var totalCount = await query.CountAsync();

                // Calculate the total number of pages
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageRequest.PageSize);

                // Apply paging (Skip and Take)
                query = query.Skip((pageRequest.PageNo - 1) * pageRequest.PageSize)
                             .Take(pageRequest.PageSize);

                var results = await query.ToListAsync();
                return (totalCount, totalPages, results);
            }, new TimeSpan(0, 0, 0, 10));

            return new PagedResult<TEntity, TPk>
            {
                Data= result.Items,
                TotalRecords= result.TotalRecords,
                TotalPages = result.TotalPages,
                PageRequest= pageRequest
            };
        }
        catch (Exception ex)
        {
            throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
        }
    }

    public async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddAsync(params TEntity[] entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public async Task AddAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        var existing = await DbSet.FindAsync(entity.Id);
        if (existing != null) DbSet.Remove(existing);
    }

    public async Task DeleteAsync(TPk id)
    {
        var typeInfo = typeof(TEntity).GetTypeInfo();
        
        var properties = DbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties;
        var key = properties.FirstOrDefault();

        var property = typeInfo.GetProperty(key?.Name ?? throw new InvalidOperationException());
        if (property != null)
        {
            var entity = Activator.CreateInstance<TEntity>();
            property.SetValue(entity, id);
            DbContext.Entry(entity).State = EntityState.Deleted;
        }
        else
        {
            var entity = DbSet.Find(id);
            if (entity != null) await DeleteAsync(entity);
        }
    }

    public async Task DeleteAsync(params TEntity[] entities)
    {
        DbSet.RemoveRange(entities);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        IEnumerable<TEntity> entities = DbContext.Set<TEntity>().Where(predicate);
        foreach (var entity in entities)
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
        }
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var trackedEntity = DbSet.Local.FirstOrDefault(e => e.Id.Equals(entity.Id));
        if (trackedEntity != null)
            DbContext.Entry(trackedEntity).State = EntityState.Detached;

        DbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(params TEntity[] entities)
    {
        DbSet.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task CommitAsync()
    {
        await DbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        DbContext?.Dispose();
    }
}
