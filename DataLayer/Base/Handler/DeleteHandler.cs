using DataLayer.Base.Response;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Base.Handler;
public class DeleteHandler
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<ResponseDto<TResult>> HandleAsync<TEntity, TResult>(
		Func<Task<TEntity?>> findEntity,
		Action<TEntity> onDelete,
		string? name = "آیتم",
		string? notFoundMessage = "آیتم مورد نظر پیدا نشد",
		string? successMessage = "آیتم با موفقیت حذف شد",
		int successCode = 200,
		CancellationToken cancellationToken = default)
		where TEntity : class
	{
		try
		{
			var entity = await findEntity();
			if (entity == null)
				return Responses.NotFound<TResult>(default, name, notFoundMessage);

			onDelete(entity);
			await _unitOfWork.CommitAsync(cancellationToken);

			return Responses.Success<TResult>(default, successMessage, successCode);
		}
		catch (Exception ex)
		{
			return Responses.Fail<TResult>(default, ex.Message);
		}
	}
}