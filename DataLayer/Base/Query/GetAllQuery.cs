using DataLayer.Base.Response;
using MediatR;

namespace DataLayer.Base.Query;
public class GetAllQuery<T> : IRequest<ResponseDto<List<T>>>
{
}
