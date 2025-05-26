using Core.Enums;
using Core.Pagination;
using DataLayer.Base.Response;
using MediatR;

namespace DataLayer.Base.Query;
public class GetPaginatedQuery<T> : IRequest<ResponseDto<PaginatedList<T>>>
{
	public string? SearchTerm { get; set; }
	public SortByEnum SortBy { get; set; } = SortByEnum.CreationDate;
	public int PageSize { get; set; } = 10;
	public int Page { get; set; } = 1;
}