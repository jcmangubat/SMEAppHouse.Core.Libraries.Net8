#nullable disable

namespace SMEAppHouse.Core.Patterns.EF.Paging.Interface;

public interface IPageRequest
{
    int Page { get; set; }
    int PageSize { get; set; }
}
