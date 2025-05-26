using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogCategoryFeatures.Response;
using _304.Net.Platform.Test.DataProvider;
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
        var category = BlogCategoryDataProvider.Row(name: "Name", id: 1, slug: "slug");


		await GetBySlugHandlerTestHelper.TestGetBySlug_Success<
            BlogCategory,
            BlogCategoryResponse,
            IBlogCategoryRepository,
            GetCategoryBySlugQueryHandler>(
                uow => new GetCategoryBySlugQueryHandler(uow),
                (handler, token) => handler.Handle(BlogCategoryDataProvider.GetBySlug(slug:"slug"), token),
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
                (handler, token) => handler.Handle(BlogCategoryDataProvider.GetBySlug(slug: "not-found"), token),
                uow => uow.BlogCategoryRepository
        );
    }
}

