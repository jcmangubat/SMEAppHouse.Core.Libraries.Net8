#nullable disable

using System.Collections.Generic;

namespace SMEAppHouse.Core.Patterns.EF.Paging;

public interface IResult
{
    PageRequest PageRequest { get; set; }
    long TotalRecords { get; set; }
    int TotalPages { get; set; }
}

public class Result : IResult
{
    public PageRequest PageRequest { get; set; }
    public long TotalRecords { get; set; }
    public int TotalPages { get; set; }
}


public class PagedResult<TEntity> : Result
    where TEntity : class
{
    public TEntity Data { get; set; }
}

public class QueryResult<TEntity> : Result
    where TEntity : class
{
    public IEnumerable<TEntity> Data { get; set; }
}
