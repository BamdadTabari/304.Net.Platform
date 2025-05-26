using Core.Base.EF;
using DataLayer.Base.Response;
using DataLayer.Base.Validator;
using DataLayer.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Base.Handler;
public class EditHandler<TCommand, TEntity>
    where TEntity : class, IBaseEntity
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<TEntity> _repository;

    public EditHandler(IUnitOfWork unitOfWork, IRepository<TEntity> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

	public async Task<ResponseDto<string>> HandleAsync(
	long id,
	List<ValidationItem>? validations,
	Func<TEntity, Task<string>> updateEntity,
	Func<TEntity, Task>? beforeUpdate = null,  // اکشن اختیاری قبل از بروزرسانی
	Func<TEntity, Task>? afterUpdate = null,   // اکشن اختیاری بعد از بروزرسانی
	string propertyName = "رکورد",
	CancellationToken cancellationToken = default
)
	{
		try
		{
			var entity = await _repository.FindSingle(x => x.id == id);

			if (entity == null)
			{
				return Responses.NotFound<string>(default, propertyName);
			}

			if (validations != null)
			{
				foreach (var validation in validations)
				{
					var isValid = await validation.Rule();
					if (isValid)
					{
						return Responses.Exist<string>(default, null, validation.Value);
					}
				}
			}

			if (beforeUpdate != null)
				await beforeUpdate(entity);

			var result = await updateEntity(entity);

			if (afterUpdate != null)
				await afterUpdate(entity);

			var committed = await _unitOfWork.CommitAsync(cancellationToken);

			if (!committed)
				return Responses.ExceptionFail(result, $"{propertyName} ویرایش نشد", 500);

			return Responses.Success<string>(result, "ویرایش با موفقیت انجام شد", 200);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "خطا در زمان ویرایش موجودیت: {Message}", ex.Message);
			return Responses.ExceptionFail<string>(default, null);
		}
	}

}
