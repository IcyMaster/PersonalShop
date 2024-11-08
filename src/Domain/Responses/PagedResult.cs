using PersonalShop.Data.Contracts;

namespace PersonalShop.Domain.Responses;

public record PagedResult<TResult>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; } = 1;
    public List<TResult> Data { get; set; } = null!;

    public PagedResult(List<TResult> data, int pageNumber, int pageSize, int totalRecord)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (totalRecord / pageSize) + 1;
        Data = data;
    }

    public static PagedResult<TResult> CreateNew(List<TResult> Data, PagedResultOffset resultOffset, int totalRecord)
    {
        return new PagedResult<TResult>(Data, resultOffset.PageNumber, resultOffset.PageSize, totalRecord);
    }
}
