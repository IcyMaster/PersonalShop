using PersonalShop.Resources.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PersonalShop.Data.Contracts;

public class PagedResultOffset
{
    [Range(1, int.MaxValue,
        ErrorMessageResourceType = typeof(PagedResultOffsetMessages),
        ErrorMessageResourceName = nameof(PagedResultOffsetMessages.PageNumberRangeError))]
    public int PageNumber { get; set; } = 1;

    [Range(1, int.MaxValue,
        ErrorMessageResourceType = typeof(PagedResultOffsetMessages),
        ErrorMessageResourceName = nameof(PagedResultOffsetMessages.PageSizeRangeError))]
    public int PageSize { get; set; } = 10;

    public PagedResultOffset()
    {
        //For default
    }

    public static ValueTask<PagedResultOffset?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (!int.TryParse(context.Request.Query["PageNumber"], out int pageNumber))
        {
            pageNumber = 1;
        }

        if (!int.TryParse(context.Request.Query["PageSize"], out int pageSize))
        {
            pageSize = 10;
        }


        var result = new PagedResultOffset
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return ValueTask.FromResult<PagedResultOffset?>(result);
    }
}
