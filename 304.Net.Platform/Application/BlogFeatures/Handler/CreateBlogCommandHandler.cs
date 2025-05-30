using _304.Net.Platform.Application.BlogFeatures.Command;
using Core.Assistant.Helpers;
using Core.EntityFramework.Models;
using DataLayer.Base.Handler;
using DataLayer.Base.Mapper;
using DataLayer.Base.Response;
using DataLayer.Repository;
using MediatR;

namespace _304.Net.Platform.Application.BlogFeatures.Handler;

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, ResponseDto<string>>
{
    private readonly CreateHandler _handler;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBlogCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = new CreateHandler(unitOfWork);
    }

    public async Task<ResponseDto<string>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
       
        var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);
        
        if (request.image_file != null)
        {
            var result = await FileHelper.UploadImage(request.image_file);
            request.image = result;
        }
        else
        {
            return Responses.NotValid<string>(data:default ,propName: "تصویر شاخص");
        }

        return await _handler.HandleAsync(
           isNameValid: async () => !await _unitOfWork.BlogRepository.ExistsAsync(x => x.name == request.name),
           isSlugValid: () => _unitOfWork.BlogRepository.ExistsAsync(x => x.slug == slug),
           propertyName: "نام مقاله",
           onCreate: async () =>
           {
               var entity = Mapper.Map<CreateBlogCommand, Blog>(request);
               await _unitOfWork.BlogRepository.AddAsync(entity);
               return slug;
           },
           createMessage: null,
           cancellationToken: cancellationToken
       );
    }
}

