﻿using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
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
public class GetAllCategoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnList_WhenCategoriesExist()
    {   
        var categories = new List<BlogCategory>
        {
		    BlogCategoryDataProvider.Row(name: "Name 1", id: 1),
			BlogCategoryDataProvider.Row(name: "Name 2", id: 2)
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

