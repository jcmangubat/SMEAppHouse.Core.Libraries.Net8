using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;

namespace SMEAppHouse.Core.Patterns.Repo.Paging;

public class PagedResult<TEntity, TPk> : PagedResultBase
    where TEntity : class, IKeyedEntity<TPk>
    where TPk : struct
{
    public IEnumerable<TEntity> Data { get; set; } = [];
}
