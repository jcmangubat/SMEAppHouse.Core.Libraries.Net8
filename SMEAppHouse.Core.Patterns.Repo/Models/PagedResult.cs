using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;

namespace SMEAppHouse.Core.Patterns.Repo.Models;

public class PagedResult<TEntity, TPk>
    where  TEntity : class, IKeyedEntity<TPk>
    where TPk : struct
{
    public IEnumerable<TEntity> Items { get; set; } = [];
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}
