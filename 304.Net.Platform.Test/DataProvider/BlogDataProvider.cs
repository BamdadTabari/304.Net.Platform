using _304.Net.Platform.Application.BlogFeatures.Command;
using _304.Net.Platform.Test.Assistant;
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
    };

}



