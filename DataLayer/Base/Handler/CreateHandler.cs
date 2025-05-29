using DataLayer.Base.Response;
using DataLayer.Repository;

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
        string propertyName = "نام",
        string slugProperty = "نامک",
        CancellationToken cancellationToken = default)
    {
        if (!await isNameValid())
            return Responses.Exist<TResult>(default, null, propertyName);

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
