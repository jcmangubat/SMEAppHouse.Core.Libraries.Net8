#nullable disable

using SMEAppHouse.Core.Patterns.EF.Paging.Interface;

namespace SMEAppHouse.Core.Patterns.EF.Paging.Response;

public class PagedResponseBase : IPageResponse
{
    public IPageRequest PageRequest { get; set; }
    public long TotalRecords { get; set; }
    public int TotalPages { get; set; }
}
