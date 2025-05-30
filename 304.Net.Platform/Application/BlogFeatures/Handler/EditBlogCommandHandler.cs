using _304.Net.Platform.Application.BlogFeatures.Command;
using Core.Assistant.Helpers;
using Core.EntityFramework.Models;
using DataLayer.Base.Handler;
using DataLayer.Base.Response;
using DataLayer.Repository;
using DataLayer.Services;
using MediatR;

namespace _304.Net.Platform.Application.BlogFeatures.Handler;

public class EditBlogCommandHandler : IRequestHandler<EditBlogCommand, ResponseDto<string>>
{
    private readonly EditHandler<EditBlogCommand,Blog> _handler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlogRepository _repository;
    public EditBlogCommandHandler(IUnitOfWork unitOfWork, IBlogRepository blogRepository)
    {
        _unitOfWork = unitOfWork;
        _repository = blogRepository;
        _handler = new EditHandler<EditBlogCommand, Blog>(unitOfWork,blogRepository);
    }

    public async Task<ResponseDto<string>> Handle(EditBlogCommand request, CancellationToken cancellationToken)
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
           id: request.id,
           isNameValid: async () => !await _unitOfWork.BlogRepository.ExistsAsync(x => x.name == request.name),
           isSlugValid: () => _unitOfWork.BlogRepository.ExistsAsync(x => x.slug == slug),
           propertyName: "مقاله",
           updateEntity: async entity =>
           {
               entity.name = request.name;
               entity.slug = request.slug ?? SlugHelper.GenerateSlug(request.name);
               entity.updated_at = request.updated_at;
               entity.description = request.description ?? "";
               entity.meta_description = request.meta_description,
               entity.blog_text = request.blog_text;

               return slug;
           },
           cancellationToken: cancellationToken
       );
    }
}

