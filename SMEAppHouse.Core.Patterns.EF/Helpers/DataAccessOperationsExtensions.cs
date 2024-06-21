using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;

namespace SMEAppHouse.Core.Patterns.EF.Helpers;

public static class DataAccessOperationsExtensions
{
    /// <summary>
    /// FILTER BY PRIMARY KEYS
    /// https://stackoverflow.com/a/42244905
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    public static IReadOnlyList<IProperty> GetPrimaryKeyProperties<T>(this DbContext dbContext)
    {
        return dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
    }

    public static Expression<Func<T, bool>> FilterByPrimaryKeyPredicate<T>(this DbContext dbContext, object[] id)
    {
        var keyProperties = dbContext.GetPrimaryKeyProperties<T>();
        var parameter = Expression.Parameter(typeof(T), "e");
        var body = keyProperties
            // e => e.PK[i] == id[i]
            .Select((p, i) => Expression.Equal(
                Expression.Property(parameter, p.Name),
                Expression.Convert(
                    Expression.PropertyOrField(Expression.Constant(new { id = id[i] }), "id"),
                    p.ClrType)))
            .Aggregate(Expression.AndAlso);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static IQueryable<TEntity> FilterByPrimaryKey<TEntity>(this DbSet<TEntity> dbSet, DbContext context, object[] id)
        where TEntity : class
    {
        return dbSet.Where(context.FilterByPrimaryKeyPredicate<TEntity>(id));
    }

    /// <summary>
    /// https://stackoverflow.com/a/42475617
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    /// <param name="context"></param>
    /// <param name="entity"></param>
    /// <param name="entryId"></param>
    public static void DetachLocal<TEntity, TPk>(this DbContext context, TEntity entity, string entryId)
        where TEntity : class, IEntityKeyed<TPk>
        where TPk : struct
    {
        var local = context.Set<TEntity>()
                        .Local
                        .FirstOrDefault(entry => entry.Id.Equals(entryId));

        if (local != null)
            context.Entry(local).State = EntityState.Detached;

        context.Entry(entity).State = EntityState.Modified;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="query"></param>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> query, int pageSize = 0, int pageNumber = 0)
        where TEntity : class, IEntity
    {
        return pageSize > 0 && pageNumber > 0 ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize) : query;
    }
}
