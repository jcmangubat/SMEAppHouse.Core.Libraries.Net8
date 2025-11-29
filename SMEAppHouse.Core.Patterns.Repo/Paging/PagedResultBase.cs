using SMEAppHouse.Core.Patterns.Repo.Paging.Interface;

namespace SMEAppHouse.Core.Patterns.Repo.Paging;

public class PagedResultBase : IPageResult
{
    public long TotalRecords { get; set; }
    public int TotalPages { get; set; }

    public required IPageRequest PageRequest { get; set; }
}
