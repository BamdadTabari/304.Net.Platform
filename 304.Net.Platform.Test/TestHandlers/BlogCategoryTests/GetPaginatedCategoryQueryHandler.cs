using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Test.GenericHandlers;
using Core.EntityFramework.Models;
using Core.Pagination;
using DataLayer.Services;


namespace _304.Net.Platform.Test.TestHandlers.BlogCategoryTests;
public class GetPaginatedCategoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenCategoriesExist()
    {
        // Arrange
        var categories = new List<BlogCategory>
        {
            new BlogCategory { id = 1, name = "Tech" },
            new BlogCategory { id = 2, name = "Health" }
        };

        var paginatedList = new PaginatedList<BlogCategory>(categories, count: 2, page: 1, pageSize: 10);

        var query = new GetPaginatedCategoryQuery
        {
            Page = 1,
            PageSize = 10,
            SearchTerm = ""
        };

        // Act + Assert
        await GetPaginatedHandlerTestHelper.TestPaginated_Success<
            BlogCategory,
            IBlogCategoryRepository,
            GetPaginatedCategoryQueryHandler,
            GetPaginatedCategoryQuery>(
                uow => new GetPaginatedCategoryQueryHandler(uow),
                (handler, q, token) => handler.Handle(q, token),
                uow => uow.BlogCategoryRepository,
                query,
                paginatedList
        );
    }

    [Fact]
    public async Task Handle_ShouldFilterBySearchTerm_WhenSearchTermProvided()
    {
        // Arrange
        var categories = new List<BlogCategory>
    {
         new BlogCategory { id = 1, name = "Tech" },
         new BlogCategory { id = 2, name = "Health" }
    };

        var paginatedList = new PaginatedList<BlogCategory>(
            categories.Where(c => c.name.Contains("Tech")).ToList(),
            count: 1, page: 1, pageSize: 10
        );

        var query = new GetPaginatedCategoryQuery
        {
            Page = 1,
            PageSize = 10,
            SearchTerm = "Tech"
        };

        // Act + Assert
        await GetPaginatedHandlerTestHelper.TestPaginated_Success<
            BlogCategory,
            IBlogCategoryRepository,
            GetPaginatedCategoryQueryHandler,
            GetPaginatedCategoryQuery>(
                uow => new GetPaginatedCategoryQueryHandler(uow),
                (handler, q, token) => handler.Handle(q, token),
                uow => uow.BlogCategoryRepository,
                query,
                paginatedList
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCategoryExists()
    {
        var paginatedList = new PaginatedList<BlogCategory>(new List<BlogCategory>(), 0, 1, 10);

        var query = new GetPaginatedCategoryQuery
        {
            Page = 1,
            PageSize = 10,
        };

        await GetPaginatedHandlerTestHelper.TestPaginated_Success<
            BlogCategory,
            IBlogCategoryRepository,
            GetPaginatedCategoryQueryHandler,
            GetPaginatedCategoryQuery>(
                uow => new GetPaginatedCategoryQueryHandler(uow),
                (handler, q, token) => handler.Handle(q, token),
                uow => uow.BlogCategoryRepository,
                query,
                paginatedList
        );
    }

}


