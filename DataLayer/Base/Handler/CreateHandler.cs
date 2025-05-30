using Core.Base.Text;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Serilog;

namespace DataLayer.Base.Handler;
public class CreateHandler
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto<TResult>> HandleAsync<TResult>(
        Func<Task<bool>>? isNameValid,
        Func<Task<bool>> isSlugValid,
        Func<Task<TResult>> onCreate,
        string propertyName = "نام",
        string slugProperty = "نامک",
        string? createMessage = "عملیات موفق بود",
        CancellationToken cancellationToken = default)
    {
        if (isNameValid != null && !await isNameValid())
            return Responses.Exist<TResult>(default, null, propertyName);

        if (await isSlugValid())
            return Responses.Exist<TResult>(default, null, slugProperty);

        try
        {
            var result = await onCreate();
            await _unitOfWork.CommitAsync(cancellationToken);
            return Responses.Success(result, createMessage, 201);
        }
        catch (Exception ex)
        {
            // لاگ‌گیری مستقیم با Serilog
            Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<TResult>(default, null);
        }
    }
    
}
