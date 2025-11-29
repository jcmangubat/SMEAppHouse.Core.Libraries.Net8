#nullable disable

namespace SMEAppHouse.Core.Patterns.Repo.Paging.Interface;

public interface IPageResult
{
    IPageRequest PageRequest { get; set; }
    long TotalRecords { get; set; }
    int TotalPages { get; set; }
}
