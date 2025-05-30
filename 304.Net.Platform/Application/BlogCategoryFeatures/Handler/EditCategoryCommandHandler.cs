using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using Core.Assistant.Helpers;
using Core.EntityFramework.Models;
using DataLayer.Base.Command;
using DataLayer.Base.Handler;
using DataLayer.Base.Response;
using DataLayer.Repository;
using MediatR;

namespace _304.Net.Platform.Application.BlogCategoryFeatures.Handler;


public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, ResponseDto<string>>
{
    private readonly EditHandler<EditCategoryCommand, BlogCategory> _handler;
    private readonly IUnitOfWork _unitOfWork;

    public EditCategoryCommandHandler(IRepository<BlogCategory> repository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = new EditHandler<EditCategoryCommand, BlogCategory>(unitOfWork, repository);
    }

    public async Task<ResponseDto<string>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);

        return await _handler.HandleAsync(
            id: request.id,
            isNameValid: async () =>
                !await _unitOfWork.BlogCategoryRepository.ExistsAsync(x => x.name == request.name && x.id != request.id),

            isSlugValid: async () =>
                await _unitOfWork.BlogCategoryRepository.ExistsAsync(x => x.slug == slug && x.id != request.id),

            updateEntity: async entity =>
            {
                entity.name = request.name;
                entity.slug = slug;
                entity.updated_at = request.updated_at;
                entity.description = request.description;
                return slug;
            },

            propertyName: "دسته‌بندی",
            cancellationToken: cancellationToken
        );
    }
}


