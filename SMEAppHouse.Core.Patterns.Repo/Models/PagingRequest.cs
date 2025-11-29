namespace SMEAppHouse.Core.Patterns.Repo.Models;

public class PagingRequest
{
    public int PageNumber { get; set; } = 1; // Default is the first page
    public int PageSize { get; set; } = 10; // Default page size
}