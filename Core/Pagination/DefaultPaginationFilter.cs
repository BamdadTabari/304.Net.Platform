using Core.Enums;

namespace Core.Pagination;
public class DefaultPaginationFilter : PaginationFilter
{
	public DefaultPaginationFilter(int pageNumber = 1, int pageSize = 10) : base(pageNumber, pageSize) { }
	public DefaultPaginationFilter() { }

	public string? Keyword { get; set; }
	public bool? BoolFilter { get; set; }
	public double? Min { get; set; }
	public double? Max { get; set; }
	public SortByEnum SortBy { get; set; } = Enums.SortByEnum.CreationDate;
}