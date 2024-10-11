#nullable disable

namespace SMEAppHouse.Core.Patterns.EF.Paging.Interface;

public interface IPageResponse
{
    IPageRequest PageRequest { get; set; }
    long TotalRecords { get; set; }
    int TotalPages { get; set; }
}
