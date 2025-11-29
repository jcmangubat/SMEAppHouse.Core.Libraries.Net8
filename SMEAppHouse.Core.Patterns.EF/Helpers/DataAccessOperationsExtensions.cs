using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;

namespace SMEAppHouse.Core.Patterns.EF.Helpers;

/// <summary>
/// Extension methods for data access operations on DbContext and DbSet.
/// </summary>
public static class DataAccessOperationsExtensions
{
    /// <summary>
    /// Gets the primary key properties for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <returns>A read-only list of primary key properties.</returns>
    /// <exception cref="ArgumentNullException">Thrown when dbContext is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the entity type is not found in the model or has no primary key.</exception>
    /// <remarks>
    /// Reference: https://stackoverflow.com/a/42244905
    /// </remarks>
    public static IReadOnlyList<IProperty> GetPrimaryKeyProperties<T>(this DbContext dbContext)
    {
        if (dbContext == null)
            throw new ArgumentNullException(nameof(dbContext));

        var entityType = dbContext.Model.FindEntityType(typeof(T));
        if (entityType == null)
            throw new InvalidOperationException($"Entity type {typeof(T).Name} is not part of the model for this context.");

        var primaryKey = entityType.FindPrimaryKey();
        if (primaryKey == null)
            throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have a primary key defined.");

        return primaryKey.Properties;
    }

    /// <summary>
    /// Creates a predicate expression that filters entities by their primary key values.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="id">An array of primary key values. The length must match the number of primary key properties.</param>
    /// <returns>A predicate expression that can be used to filter entities by primary key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when dbContext or id is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the id array length doesn't match the number of primary key properties.</exception>
    public static Expression<Func<T, bool>> FilterByPrimaryKeyPredicate<T>(this DbContext dbContext, object[] id)
    {
        if (dbContext == null)
            throw new ArgumentNullException(nameof(dbContext));
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        var keyProperties = dbContext.GetPrimaryKeyProperties<T>();
        if (id.Length != keyProperties.Count)
            throw new ArgumentException($"The number of ID values ({id.Length}) must match the number of primary key properties ({keyProperties.Count}).", nameof(id));

        var parameter = Expression.Parameter(typeof(T), "e");
        var body = keyProperties
            // e => e.PK[i] == id[i]
            .Select((p, i) =>
            {
                var propertyExpression = Expression.Property(parameter, p.Name);
                var value = id[i];
                
                // Convert the value to the property's CLR type if needed
                Expression valueExpression;
                if (value != null && p.ClrType.IsAssignableFrom(value.GetType()))
                {
                    valueExpression = Expression.Constant(value, p.ClrType);
                }
                else
                {
                    // Use Convert to handle type conversion
                    var constantValue = Expression.Constant(value);
                    valueExpression = Expression.Convert(constantValue, p.ClrType);
                }
                
                return Expression.Equal(propertyExpression, valueExpression);
            })
            .Aggregate(Expression.AndAlso);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    /// <summary>
    /// Filters a DbSet by primary key values.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="dbSet">The database set to filter.</param>
    /// <param name="context">The database context.</param>
    /// <param name="id">An array of primary key values.</param>
    /// <returns>A filtered queryable.</returns>
    /// <exception cref="ArgumentNullException">Thrown when dbSet, context, or id is null.</exception>
    public static IQueryable<TEntity> FilterByPrimaryKey<TEntity>(this DbSet<TEntity> dbSet, DbContext context, object[] id)
        where TEntity : class
    {
        if (dbSet == null)
            throw new ArgumentNullException(nameof(dbSet));
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        return dbSet.Where(context.FilterByPrimaryKeyPredicate<TEntity>(id));
    }

    /// <summary>
    /// Detaches a local entity from the context and attaches the provided entity as modified.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPk">The primary key type.</typeparam>
    /// <param name="context">The database context.</param>
    /// <param name="entity">The entity to attach as modified.</param>
    /// <param name="entryId">The ID of the local entity to detach. Can be a string representation of the ID or the ID value itself.</param>
    /// <exception cref="ArgumentNullException">Thrown when context, entity, or entryId is null.</exception>
    /// <exception cref="ArgumentException">Thrown when entryId cannot be converted to TPk.</exception>
    /// <remarks>
    /// Reference: https://stackoverflow.com/a/42475617
    /// </remarks>
    public static void DetachLocal<TEntity, TPk>(this DbContext context, TEntity entity, string entryId)
        where TEntity : class, IKeyedEntity<TPk>
        where TPk : struct
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (string.IsNullOrWhiteSpace(entryId))
            throw new ArgumentNullException(nameof(entryId));

        // Convert string entryId to TPk type
        TPk parsedId;
        try
        {
            if (typeof(TPk) == typeof(Guid))
            {
                parsedId = (TPk)(object)Guid.Parse(entryId);
            }
            else if (typeof(TPk).IsEnum)
            {
                parsedId = (TPk)Enum.Parse(typeof(TPk), entryId);
            }
            else
            {
                parsedId = (TPk)Convert.ChangeType(entryId, typeof(TPk));
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Cannot convert '{entryId}' to type {typeof(TPk).Name}.", nameof(entryId), ex);
        }

        var local = context.Set<TEntity>()
                        .Local
                        .FirstOrDefault(entry => entry.Id.Equals(parsedId));

        if (local != null)
            context.Entry(local).State = EntityState.Detached;

        context.Entry(entity).State = EntityState.Modified;
    }

    /// <summary>
    /// Applies paging to a queryable collection.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="query">The queryable to page.</param>
    /// <param name="pageSize">The number of items per page. Must be greater than 0 for paging to be applied.</param>
    /// <param name="pageNumber">The page number (1-based). Must be greater than 0 for paging to be applied.</param>
    /// <returns>A paged queryable, or the original query if pageSize or pageNumber is 0 or less.</returns>
    /// <exception cref="ArgumentNullException">Thrown when query is null.</exception>
    public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> query, int pageSize = 0, int pageNumber = 0)
        where TEntity : class, IEntity
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        return pageSize > 0 && pageNumber > 0 
            ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize) 
            : query;
    }
}
