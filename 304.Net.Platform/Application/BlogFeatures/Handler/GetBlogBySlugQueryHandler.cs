using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogCategoryFeatures.Response;
using _304.Net.Platform.Application.BlogFeatures.Query;
using _304.Net.Platform.Application.BlogFeatures.Response;
using Core.EntityFramework.Models;
using DataLayer.Base.Handler;
using DataLayer.Base.Response;
using DataLayer.Repository;
using MediatR;

namespace _304.Net.Platform.Application.BlogFeatures.Handler;

public class GetBlogBySlugQueryHandler : IRequestHandler<GetBlogBySlugQuery, ResponseDto<BlogResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly GetBySlugHandler _handler;

	public GetBlogBySlugQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_handler = new GetBySlugHandler(unitOfWork);
	}

	public async Task<ResponseDto<BlogResponse>> Handle(GetBlogBySlugQuery request, CancellationToken cancellationToken)
	{
		return await _handler.Handle<Blog, BlogResponse>(
			async uow => await uow.BlogRepository.FindSingle(x => x.slug == request.slug, "blog_category"),
			"مقاله",
			null
		);
	}
}