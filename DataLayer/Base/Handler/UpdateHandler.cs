using DataLayer.Base.Response;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Base.Handler;
public class UpdateHandler
{
	private readonly IUnitOfWork _unitOfWork;

	public UpdateHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<ResponseDto<string>> HandleAsync<TEntity>(
		Func<Task<TEntity?>> findEntity,
		Func<TEntity, Task<bool>> isNameDuplicated,
		Func<TEntity, Task<bool>> isSlugDuplicated,
		Func<TEntity, Task> onUpdate,
		Func<TEntity, Task> update,
		string nameProperty = "نام",
		string slugProperty = "نامک",
		CancellationToken cancellationToken = default)
		where TEntity : class
	{
		var entity = await findEntity();
		if (entity == null)
			return Responses.NotFound(nameProperty);

		if (await isNameDuplicated(entity))
			return Responses.Exist<string>(null,null, nameProperty);

		if (await isSlugDuplicated(entity))
			return Responses.Exist<string>(null, null, slugProperty);

		await onUpdate(entity);
		await update(entity);
		await _unitOfWork.CommitAsync(cancellationToken);

		return Responses.Success<string>(null, "عملیات موفقیت آمیز بود", 204);
	}
}