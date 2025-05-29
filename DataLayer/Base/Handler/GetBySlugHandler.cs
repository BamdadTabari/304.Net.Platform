using DataLayer.Base.Response;
using DataLayer.Repository;

namespace DataLayer.Base.Handler;
public class GetBySlugHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBySlugHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto<TDto>> Handle<TEntity, TDto>(
        Func<IUnitOfWork, Task<TEntity?>> fetchFunc,
        string? name,
        string? notFoundMessage)
        where TEntity : class
        where TDto : class, new()
    {
        try
        {
            var entity = await fetchFunc(_unitOfWork);
            if (entity == null)
                return Responses.NotFound<TDto>(default, name);

            var dto = Mapper.Mapper.Map<TEntity, TDto>(entity);
            return Responses.Data(dto);
        }
        catch (Exception ex)
        {
            return Responses.Fail<TDto>(default, ex.Message);
        }
    }
}
