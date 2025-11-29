#nullable disable

namespace SMEAppHouse.Core.Patterns.Repo.Paging;

/// <summary>
/// Represents a paginated response containing a collection of entities.
/// </summary>
/// <typeparam name="TEntity">The type of the entities in the response.</typeparam>
/// <remarks>
/// The <see cref="QueryableResult{TEntity}"/> class extends the <see cref="PagedResponseBase"/> class,
/// providing additional properties for managing a collection of entities retrieved from a query.
/// It includes pagination information as well as the data itself.
public class QueryableResult<TEntity> : PagedResultBase
    where TEntity : class
{
    public IQueryable<TEntity> Data { get; set; } = new List<TEntity>().AsQueryable();
}
