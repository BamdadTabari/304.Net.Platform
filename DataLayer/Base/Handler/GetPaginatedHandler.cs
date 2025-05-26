using Core.Pagination;
using DataLayer.Base.Response;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Base.Handler;
public class GetPaginatedHandler
{
	private readonly IUnitOfWork _unitOfWork;

	public GetPaginatedHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public ResponseDto<PaginatedList<TEntity>> Handle<TEntity>(
		Func<IUnitOfWork, PaginatedList<TEntity>> getRepo,
		DefaultPaginationFilter filter)
		where TEntity : class
	{
		try
		{
			var result = getRepo(_unitOfWork);
			return Responses.Data(result);
		}
		catch (Exception ex)
		{
			return Responses.Fail<PaginatedList<TEntity>>(default , ex.Message);
		}
	}
}