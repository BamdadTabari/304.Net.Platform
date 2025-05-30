using Core.Pagination;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Serilog;

namespace DataLayer.Base.Handler;
public class GetPaginatedHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto<PaginatedList<TEntity>>> Handle<TEntity>(
        Func<IUnitOfWork, Task<PaginatedList<TEntity>>> getRepo)
        where TEntity : class
    {
        try
        {
            var result = await getRepo(_unitOfWork);
            return Responses.Data(result);
        }
        catch (Exception ex)
        {
            // لاگ‌گیری مستقیم با Serilog
            Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<PaginatedList<TEntity>>(default, null);
        }
    }
}