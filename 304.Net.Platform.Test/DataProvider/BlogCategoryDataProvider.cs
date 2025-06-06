﻿using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogCategoryFeatures.Response;
using Core.EntityFramework.Models;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace _304.Net.Platform.Test.DataProvider;
public static class BlogCategoryDataProvider
{
    public static CreateCategoryCommand Create(string name = "name")
        => new CreateCategoryCommand()
        {
            name = name,
            created_at = DateTime.Now,
            description = "description",
            slug = null,
            updated_at = DateTime.Now,
        };

    public static EditCategoryCommand Edit(string name = "name", long id = 1)
        => new EditCategoryCommand()
        {
            id = id,
            name = name,
            description = "description",
            slug = null,
            updated_at = DateTime.Now,
        };


	public static BlogCategory Row(string name = "name", long id = 1, string slug = "slug")
	=> new BlogCategory()
	{
		id = id,
		name = name,
		description = "description",
		slug = slug,
		updated_at = DateTime.Now,
	};

	public static DeleteCategoryCommand Delete(long id = 1)
        => new DeleteCategoryCommand()
        {
            id = id,
        };

    public static GetCategoryBySlugQuery GetBySlug(string slug = "slug")
    => new GetCategoryBySlugQuery()
    {
        slug = slug,
    };

	public static BlogCategoryResponse GetOne(string slug = "slug", string name = "name")
		=> new BlogCategoryResponse()
		{
			id = 1,
			name = name,
			slug = slug,
			description = "Tech Category"
		};

	public static GetPaginatedCategoryQuery GetByQueryFilter(string searchTerm = "")
	=> new GetPaginatedCategoryQuery()
	{
		Page = 1,
		PageSize = 10,
		SearchTerm = searchTerm,
	};

	public static PaginatedList<BlogCategory> GetPaginatedList()
	=> new PaginatedList<BlogCategory>(new List<BlogCategory>
		{
			new BlogCategory { id = 1, name = "Tech" },
			new BlogCategory { id = 2, name = "Health" }
		}
    , count: 2, page: 1, pageSize: 10);
}
