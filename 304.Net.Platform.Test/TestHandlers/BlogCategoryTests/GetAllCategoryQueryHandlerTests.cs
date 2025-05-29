using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogCategoryFeatures.Response;
using _304.Net.Platform.Test.GenericHandlers;
using Core.EntityFramework.Models;
using DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace _304.Net.Platform.Test.TestHandlers.BlogCategoryTests;
public class GetAllCategoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnList_WhenCategoriesExist()
    {   
        var categories = new List<BlogCategory>
        {
            new BlogCategory { id = 1, name = "Tech", slug = "tech" , description="", created_at= DateTime.Now, updated_at = DateTime.Now},
            new BlogCategory { id = 2, name = "Health", slug = "health" , description="", created_at= DateTime.Now, updated_at = DateTime.Now}
        };

        await GetAllHandlerTestHelper.TestHandle_Success
               <BlogCategory, BlogCategoryResponse, IBlogCategoryRepository, GetAllCategoryQueryHandler>(
               handlerFactory: unitOfWork => new GetAllCategoryQueryHandler(unitOfWork),
               execute: (handler, ct) => handler.Handle(new GetAllCategoryQuery(), ct),
               repoSelector: u => u.BlogCategoryRepository,
               entities: categories);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenExceptionThrown()
    {
        await GetAllHandlerTestHelper.TestHandle_FailOnException<BlogCategory, BlogCategoryResponse, IBlogCategoryRepository, GetAllCategoryQueryHandler>(
            handlerFactory: uow => new GetAllCategoryQueryHandler(uow),
            execute: (handler, ct) => handler.Handle(new GetAllCategoryQuery(), ct),
            repoSelector: uow => uow.BlogCategoryRepository);
    }
}

