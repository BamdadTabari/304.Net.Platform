using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogCategoryFeatures.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace _304.Net.Platform.Test.DataProvider;
internal static class BlogCategoryDataProvider
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

    public static DeleteCategoryCommand Delete(long id = 1)
        => new DeleteCategoryCommand()
        {
            id = id,
        };

    public static GetCategoryBySlugQuery GetBySLug(string slug = "slug")
    => new GetCategoryBySlugQuery()
    {
        slug = slug,
    };

    public static BlogCategoryResponse GetOne(string slug = "slug")
        => new BlogCategoryResponse()
        {
            slug = slug,
        };
}
