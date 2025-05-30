using Core.Base.EF;
using DataLayer.Base.Response;
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
        Func<Task<bool>>? isNameValid,
        Func<Task<bool>> isSlugValid,
        Func<TEntity, Task<string>> updateEntity,
        string propertyName = "رکورد",
        string slugProperty = "نامک",
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var entity = await _repository.FindSingle(x => x.id == id);

            if (entity == null)
            {
                return new ResponseDto<string>
                {
                    is_success = false,
                    response_code = 404,
                    message = $"{propertyName} پیدا نشد",
                    data = null
                };
            }
            if (isNameValid != null && !await isNameValid())
                return Responses.Exist<string>(default, null, propertyName);

            if (await isSlugValid())
                return Responses.Exist<string>(default, null, slugProperty);
            var result =  await updateEntity(entity);

            var committed = await _unitOfWork.CommitAsync(cancellationToken);

            if (!committed)
                return Responses.ExceptionFail(result, $"{propertyName} ویرایش نشد", 500);

            return Responses.Success<string>(result, "ویرایش با موفقیت انجام شد", 204);
        }
        catch (Exception ex)
        {
            // لاگ‌گیری مستقیم با Serilog
            Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<string>(default, null);
        }
    }
}
