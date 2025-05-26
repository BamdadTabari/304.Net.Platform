using DataLayer.Base.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Base.Query;
public class GetAllQuery<T> : IRequest<ResponseDto<List<T>>>
{
}
