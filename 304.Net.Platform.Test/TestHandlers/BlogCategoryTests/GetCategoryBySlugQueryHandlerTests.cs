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
public class GetCategoryBySlugQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnData_WhenCategoryExists()
    {
        var category = new BlogCategory
        {
            id = 1,
            name = "Tech",
            slug = "tech",
            description = "Tech Category"
        };

        await GetBySlugHandlerTestHelper.TestGetBySlug_Success<
            BlogCategory,
            BlogCategoryResponse,
            IBlogCategoryRepository,
            GetCategoryBySlugQueryHandler>(
                uow => new GetCategoryBySlugQueryHandler(uow),
                (handler, token) => handler.Handle(new GetCategoryBySlugQuery { slug = "tech" }, token),
                uow => uow.BlogCategoryRepository,
                category
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        await GetBySlugHandlerTestHelper.TestGetBySlug_NotFound<
            BlogCategory,
            BlogCategoryResponse,
            IBlogCategoryRepository,
            GetCategoryBySlugQueryHandler>(
                uow => new GetCategoryBySlugQueryHandler(uow),
                (handler, token) => handler.Handle(new GetCategoryBySlugQuery { slug = "not-found" }, token),
                uow => uow.BlogCategoryRepository
        );
    }
}

