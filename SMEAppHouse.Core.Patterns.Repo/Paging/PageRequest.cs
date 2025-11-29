using SMEAppHouse.Core.Patterns.Repo.Paging.Interface;

namespace SMEAppHouse.Core.Patterns.Repo.Paging;

public class PageRequest : IPageRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
