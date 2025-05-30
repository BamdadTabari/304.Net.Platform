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
        Func<TEntity, Task> updateEntity,
        string propertyName = "رکورد",
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

            await updateEntity(entity);

            var committed = await _unitOfWork.CommitAsync(cancellationToken);

            if (!committed)
            {
                return new ResponseDto<string>
                {
                    is_success = false,
                    response_code = 500,
                    message = $"{propertyName} ویرایش نشد",
                    data = null
                };
            }

            return new ResponseDto<string>
            {
                is_success = true,
                response_code = 200,
                message = "ویرایش با موفقیت انجام شد",
                data = entity.id.ToString()
            };
        }
        catch (Exception ex)
        {
            // لاگ‌گیری مستقیم با Serilog
            Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<string>(default, null);
        }
    }
}
