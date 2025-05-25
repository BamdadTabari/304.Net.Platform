using Core.Enums;

namespace Core.Pagination;
public class PaginationFilter
{
	private const int MinPageNumber = 1;
	private const int MaxPageSize = 200;

	public PaginationFilter(int pageNumber = MinPageNumber, int pageSize = 10, SortByEnum sortBy = SortByEnum.CreationDate)
	{
		Page = pageNumber > 0 ? pageNumber : MinPageNumber;
		PageSize = pageSize > 0 && pageSize <= MaxPageSize ? pageSize : MaxPageSize;
		SortByEnum = sortBy;
	}

	public int Page { get; set; }
	public int PageSize { get; set; }
	public int TotalPageCount { get; set; }
	public SortByEnum SortByEnum { get; set; }
}