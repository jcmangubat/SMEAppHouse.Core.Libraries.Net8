#nullable disable

namespace SMEAppHouse.Core.Patterns.EF.Paging.Response;


public class PagedResponse<TEntity> : PagedResponseBase
    where TEntity : class
{
    public IEnumerable<TEntity> Data { get; set; }
}
