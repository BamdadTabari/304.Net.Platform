using DataLayer.Base.Response;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using DataLayer.Base.Mapper;
using Core.EntityFramework.Models;

namespace DataLayer.Base.Handler;
public class GetAllHandler
{
	private readonly IUnitOfWork _unitOfWork;

	public GetAllHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public ResponseDto<List<TDto>> Handle<TEntity, TDto>(
		Func<IQueryable<TEntity>> getEntities)
		where TEntity : class
		where TDto : class, new()
	{
		try
		{
			var entities = getEntities().ToList();
			var dtoList = Mapper.Mapper.Map<List<TEntity>, List<TDto>>(entities);

			return Responses.Data(dtoList);
		}
		catch (Exception ex)
		{
			return Responses.Fail<List<TDto>>( default,ex.Message);
		}
	}
}