using Core.Base.Text;
using DataLayer.Base.Response;
using DataLayer.Base.Validator;
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
		List<ValidationItem>? validations,
		Func<Task<TResult>> onCreate,
        string? createMessage = "عملیات موفق بود",
        CancellationToken cancellationToken = default)
    {
		if (validations != null)
		{
			foreach (var validation in validations)
			{
				var isValid = await validation.Rule();
				if (isValid)
				{
					return Responses.Exist<TResult>(default, null, validation.Value);
				}
			}
		}

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
