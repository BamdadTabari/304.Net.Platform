﻿using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using Core.EntityFramework.Models;
using DataLayer.Base.Handler;
using DataLayer.Base.Response;
using DataLayer.Repository;
using MediatR;

namespace _304.Net.Platform.Application.BlogCategoryFeatures.Handler;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ResponseDto<string>>
{
    private readonly DeleteHandler _handler;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = new DeleteHandler(unitOfWork);
    }

    public async Task<ResponseDto<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _handler.HandleAsync<BlogCategory, string>(
            findEntity: () => _unitOfWork.BlogCategoryRepository.FindSingle(x => x.id == request.id),
            onDelete: entity => _unitOfWork.BlogCategoryRepository.Remove(entity),
            name: "دسته‌بندی",
            notFoundMessage: "دسته‌بندی پیدا نشد",
            successMessage: "دسته‌بندی با موفقیت حذف شد",
            cancellationToken: cancellationToken
        );
    }
}