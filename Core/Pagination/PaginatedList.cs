namespace Core.Pagination;
public class PaginatedList<T>
{
    public int Page { get; set; }               // برای سازگاری با فیلترهای قبلی
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public List<T> Data { get; set; }

    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PaginatedList(IEnumerable<T> items, int count, int page, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize > 0 ? pageSize : 10;   // جلوگیری از تقسیم بر صفر
        CurrentPage = page > 0 ? page : 1;
        Page = CurrentPage;

        TotalPages = TotalCount > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;

        Data = items?.ToList() ?? new List<T>();
    }
}

