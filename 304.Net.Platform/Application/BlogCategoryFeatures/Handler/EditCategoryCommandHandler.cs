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

    public EditCategoryCommandHandler(IRepository<BlogCategory> repository, IUnitOfWork unitOfWork)
    {
        _handler = new EditHandler<EditCategoryCommand, BlogCategory>(unitOfWork, repository);
    }

    public async Task<ResponseDto<string>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _handler.HandleAsync(
            id: request.id,
            updateEntity: async entity =>
            {
                entity.name = request.name;
                entity.slug = request.slug ?? SlugHelper.GenerateSlug(request.name);
                entity.updated_at = request.updated_at;
                entity.description = request.description;
                await Task.CompletedTask;
            },
            propertyName: "دسته‌بندی",
            cancellationToken: cancellationToken
        );
    }
}


