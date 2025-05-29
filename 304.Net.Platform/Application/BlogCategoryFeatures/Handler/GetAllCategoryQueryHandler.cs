using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogCategoryFeatures.Response;
using Core.EntityFramework.Models;
using DataLayer.Base.Handler;
using DataLayer.Base.Response;
using DataLayer.Repository;
using MediatR;

namespace _304.Net.Platform.Application.BlogCategoryFeatures.Handler;

public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, ResponseDto<List<BlogCategoryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetAllHandler _handler;

    public GetAllCategoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = new GetAllHandler(unitOfWork);
    }

    public Task<ResponseDto<List<BlogCategoryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            _handler.Handle<BlogCategory, BlogCategoryResponse>(
                _unitOfWork.BlogCategoryRepository.FindList()
            )
        );
    }
}