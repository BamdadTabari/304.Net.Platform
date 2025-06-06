﻿using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogCategoryFeatures.Response;
using Core.EntityFramework.Models;
using DataLayer.Base.Handler;
using DataLayer.Base.Response;
using DataLayer.Repository;
using MediatR;

namespace _304.Net.Platform.Application.BlogCategoryFeatures.Handler;

public class GetCategoryBySlugQueryHandler : IRequestHandler<GetCategoryBySlugQuery, ResponseDto<BlogCategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetBySlugHandler _handler;

    public GetCategoryBySlugQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = new GetBySlugHandler(unitOfWork);
    }

    public async Task<ResponseDto<BlogCategoryResponse>> Handle(GetCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _handler.Handle<BlogCategory, BlogCategoryResponse>(
            async uow => await uow.BlogCategoryRepository.FindSingle(x => x.slug == request.slug),
            "دسته‌بندی",
            null
        );
    }
}
