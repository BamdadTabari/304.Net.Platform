using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogCategoryFeatures.Response;
using _304.Net.Platform.Application.BlogFeatures.Command;
using _304.Net.Platform.Test.Assistant;
using Core.EntityFramework.Models;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace _304.Net.Platform.Test.DataProvider;
internal static class BlogDataProvider
{
    public static CreateBlogCommand Create(string name = "test")
        => new CreateBlogCommand()
        {
            name = name,
            description = "Test",
            image_alt = "Test",
            image_file = Files.CreateFakeFormFile(),
            blog_category_id = 1,
            blog_text = "Test",
            created_at = DateTime.Now,
            updated_at = DateTime.Now,
            estimated_read_time = 5,
            keywords = "a,b,c",
            show_blog = true,
            meta_description = "Test",
            slug = null,
        };

    public static EditBlogCommand Edit(string name = "test", long id = 1)
    => new EditBlogCommand()
    {
        id = id,
        name = name,
        description = "Test",
        image_alt = "Test",
        image_file = Files.CreateFakeFormFile(),
        blog_category_id = 1,
        blog_text = "Test",
        updated_at = DateTime.Now,
        estimated_read_time = 5,
        keywords = "a,b,c",
        show_blog = true,
        meta_description = "Test",
        slug = null,
		image = "test.jpg",
    };


	public static Blog Row(string name = "name", long id = 1, string slug = "slug")
	=> new Blog()
	{
		id = id,
		name = name,
		description = "Test",
		image_alt = "Test",
		blog_category_id = 1,
		blog_text = "Test",
		updated_at = DateTime.Now,
		estimated_read_time = 5,
		keywords = "a,b,c",
		show_blog = true,
		meta_description = "Test",
		slug = slug,
		image = "test.jpg",
		created_at = DateTime.Now,
	};

	public static DeleteBlogCommand Delete(long id = 1)
		=> new DeleteBlogCommand()
		{
			id = id,
		};

	//public static GetCategoryBySlugQuery GetBySlug(string slug = "slug")
	//=> new GetCategoryBySlugQuery()
	//{
	//	slug = slug,
	//};

	//public static BlogCategoryResponse GetOne(string slug = "slug", string name = "name")
	//	=> new BlogCategoryResponse()
	//	{
	//		id = 1,
	//		name = name,
	//		slug = slug,
	//		description = "Tech Category"
	//	};

	//public static GetPaginatedCategoryQuery GetByQueryFilter(string searchTerm = "")
	//=> new GetPaginatedCategoryQuery()
	//{
	//	Page = 1,
	//	PageSize = 10,
	//	SearchTerm = searchTerm,
	//};

	//public static PaginatedList<BlogCategory> GetPaginatedList()
	//=> new PaginatedList<BlogCategory>(new List<BlogCategory>
	//	{
	//		new BlogCategory { id = 1, name = "Tech" },
	//		new BlogCategory { id = 2, name = "Health" }
	//	}
	//, count: 2, page: 1, pageSize: 10);

}



