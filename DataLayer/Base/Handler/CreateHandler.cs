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

	public async Task<ResponseDto<string>> HandleAsync(
		Func<Task<bool>> isNameValid,
		Func<Task<bool>> isSlugValid,
		string propertyName,
		Func<Task<string>> onCreate,
		CancellationToken cancellationToken = default)
	{
		if (!await isNameValid())
			return Responses.Exist<string>(null, null, propertyName);

		if (await isSlugValid())
			return Responses.Exist<string>(null,null, "نامک");

		try
		{
			var result = await onCreate();
			await _unitOfWork.CommitAsync(cancellationToken);
			return Responses.Success(result);
		}
		catch (Exception ex)
		{
			return Responses.Fail("خطا در انجام عملیات: " + ex.Message);
		}
	}
}
