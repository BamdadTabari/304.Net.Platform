﻿using _304.Net.Platform.Application.BlogFeatures.Handler;
using _304.Net.Platform.Application.BlogFeatures.Query;
using _304.Net.Platform.Application.BlogFeatures.Handler;
using _304.Net.Platform.Application.BlogFeatures.Query;
using _304.Net.Platform.Test.DataProvider;
using _304.Net.Platform.Test.GenericHandlers;
using Core.EntityFramework.Models;
using Core.Pagination;
using DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace _304.Net.Platform.Test.TestHandlers.BlogTests;
public class GetPaginatedBlogQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnPaginatedList_WhenCategoriesExist()
	{
		var paginatedList = BlogDataProvider.GetPaginatedList();

		var query = BlogDataProvider.GetByQueryFilter();

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Blog,
			IBlogRepository,
			GetPaginatedBlogQueryHandler,
			GetPaginatedBlogQuery>(
				uow => new GetPaginatedBlogQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.BlogRepository,
				query,
				paginatedList,
				includes: new[] { "blog_category"}
		);
	}


	[Fact]
	public async Task Handle_ShouldFilterBySearchTerm_WhenSearchTermProvided()
	{
		// Arrange
		var categories = new List<Blog>
		{
			BlogDataProvider.Row(id: 1, name: "Health"),
			BlogDataProvider.Row(id: 1, name: "Tech")
		};

		var paginatedList = new PaginatedList<Blog>(
			categories.Where(c => c.name.Contains("Tech")).ToList(),
			count: 1, page: 1, pageSize: 10
		);

		var query = BlogDataProvider.GetByQueryFilter(searchTerm: "Tech");

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Blog,
			IBlogRepository,
			GetPaginatedBlogQueryHandler,
			GetPaginatedBlogQuery>(
				uow => new GetPaginatedBlogQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.BlogRepository,
				query,
				paginatedList,
				includes: new[] { "blog_category" }
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnEmptyList_WhenNoBlogExists()
	{
		var paginatedList = new PaginatedList<Blog>(new List<Blog>(), 0, 1, 10);

		var query = BlogDataProvider.GetByQueryFilter();

		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Blog,
			IBlogRepository,
			GetPaginatedBlogQueryHandler,
			GetPaginatedBlogQuery>(
				uow => new GetPaginatedBlogQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.BlogRepository,
				query,
				paginatedList,
				includes: new[] { "blog_category" }
		);
	}
}
