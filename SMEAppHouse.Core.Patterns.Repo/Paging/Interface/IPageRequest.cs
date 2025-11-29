#nullable disable

namespace SMEAppHouse.Core.Patterns.Repo.Paging.Interface;

public interface IPageRequest
{
    int PageNo { get; set; }
    int PageSize { get; set; }
}
