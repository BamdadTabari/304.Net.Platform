using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
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
            // Define the directory for uploads 
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "images");

            // Create directory if not Exist
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Build file name
            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.image_file.FileName);
            var imagePath = Path.Combine(uploadPath, newFileName);

            // Save Image
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await request.image_file.CopyToAsync(stream);
            }
            request.image = imagePath;
        }
        else
        {
            return new ResponseDto<string>()
            {
                data = null,
                is_success = false,
                message = "لطفا تصویر شاخص را آپلود کنید",
                response_code = 400
            };
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

