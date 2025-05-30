using DataLayer.Base.Response;
using DataLayer.Repository;
using Serilog;

namespace DataLayer.Base.Handler;
public class GetAllHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ResponseDto<List<TDto>> Handle<TEntity, TDto>(
        List<TEntity> entities)
        where TEntity : class
        where TDto : class, new()
    {
        try
        {
            var dtoList = Mapper.Mapper.Map<List<TEntity>, List<TDto>>(entities);

            return Responses.Data(dtoList);
        }
        catch (Exception ex)
        {
            // لاگ‌گیری مستقیم با Serilog
            Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<List<TDto>>(default, null);
        }
    }
}