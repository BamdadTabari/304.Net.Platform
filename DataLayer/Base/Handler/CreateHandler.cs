using DataLayer.Base.Response;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataLayer.Base.Handler;
public class CreateHandler
{
	private readonly IUnitOfWork _unitOfWork;

	public CreateHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<ResponseDto<TResult>> HandleAsync<TResult>(
		Func<Task<bool>> isNameValid,
		Func<Task<bool>> isSlugValid,
		Func<Task<TResult>> onCreate,
		string nameProperty = "نام",
		string slugProperty = "نامک",
		CancellationToken cancellationToken = default)
	{
		if (!await isNameValid())
			return Responses.Exist<TResult>(default, null, nameProperty);

		if (await isSlugValid())
			return Responses.Exist<TResult>(default, null, slugProperty);

		try
		{
			var result = await onCreate();
			await _unitOfWork.CommitAsync(cancellationToken);
			return Responses.Success(result, null, 201);
		}
		catch (Exception ex)
		{
			return Responses.Fail<TResult>(default, ex.Message);
		}
	}
}
		