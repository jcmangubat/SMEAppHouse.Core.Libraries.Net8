﻿using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SMEAppHouse.Core.CodeKits.Helpers;
using SMEAppHouse.Core.CodeKits.Tools;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;

namespace SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="keyValues"></param>
    /// <returns></returns>
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

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate )
    {
        IQueryable<TEntity> query = DbSet.Where(predicate);
        return await query.OrderBy(p => p.Id).FirstOrDefaultAsync();
    }

    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate = null
        , Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null
        , bool disableTracking = true)
    {
        IQueryable<TEntity> query = DbSet;
        if (disableTracking) query = query.AsNoTracking();
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        return await query.OrderBy(p => p.Id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await GetListAsync(filter, null, null);
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                int fetchSize = 0, bool disableTracking = true)
    {
        try
        {
            var result = await RetryCodeKit.Do(async () =>
            {
                IQueryable<TEntity> query = DbSet;

                if (disableTracking) query = query.AsNoTracking();
                if (filter != null) query = query.Where(filter);
                if (include != null) query = include(query);

                //if (!string.IsNullOrEmpty(includeProperties))
                //    query = includeProperties
                //        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                //        .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                var qryable = orderBy?.Invoke(query) ?? query;

                if (fetchSize > 0)
                {
                    var sizedResults = (await query.ToListAsync()).OrderBy(x => Guid.NewGuid()).Take(fetchSize);
                    return sizedResults;
                }

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
        var key = DbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
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
            this.DbContext.Entry(trackedEntity).State = EntityState.Detached;

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
