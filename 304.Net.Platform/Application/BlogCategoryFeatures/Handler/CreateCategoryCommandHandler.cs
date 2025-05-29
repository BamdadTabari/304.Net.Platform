using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using Core.Assistant.Helpers;
using Core.EntityFramework.Models;
using DataLayer.Base.Handler;
using DataLayer.Base.Mapper;
using DataLayer.Base.Response;
using DataLayer.Repository;
using MediatR;
namespace _304.Net.Platform.Application.BlogCategoryFeatures.Handler;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseDto<string>>
{
    private readonly CreateHandler _handler;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = new CreateHandler(unitOfWork);
    }

    public async Task<ResponseDto<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);

        return await _handler.HandleAsync(
            isNameValid: () => _unitOfWork.BlogCategoryRepository
                .ExistsAsync(x => x.name == request.name)
                .ContinueWith(t => !t.Result),

            isSlugValid: () => _unitOfWork.BlogCategoryRepository
                .ExistsAsync(x => x.slug == slug),

            propertyName: "نام دسته‌بندی",

            onCreate: async () =>
            {
                var entity = Mapper.Map<CreateCategoryCommand, BlogCategory>(request);

                await _unitOfWork.BlogCategoryRepository.AddAsync(entity);
                return slug;
            },

            cancellationToken: cancellationToken
        );
    }
}