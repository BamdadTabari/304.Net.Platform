using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using Core.Base.EF;
using Core.EntityFramework.Models;
using Core.Pagination;
using DataLayer.Base.Response;
using DataLayer.Repository;
using DataLayer.Services;
using Moq;
using System.Linq.Expressions;

namespace _304.Net.Platform.Test.GenericHandlers;
public static class GetPaginatedHandlerTestHelper
{
    public static async Task TestPaginated_Success<TEntity, TRepository, THandler, TQuery>(
        Func<IUnitOfWork, THandler> handlerFactory,
        Func<THandler, TQuery, CancellationToken, Task<ResponseDto<PaginatedList<TEntity>>>> execute,
        Expression<Func<IUnitOfWork, TRepository>> repoSelector,
        TQuery query,
        PaginatedList<TEntity> expectedList)
        where TEntity : class, IBaseEntity
        where TRepository : class, IRepository<TEntity>
        where THandler : class
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

        repoMock.Setup(r => r.GetPagedResultAsync(
            It.IsAny<DefaultPaginationFilter>(),
            It.IsAny<Expression<Func<TEntity, bool>>>(),
            It.IsAny<string[]>()
        )).ReturnsAsync(expectedList);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, query, CancellationToken.None);

        Assert.True(result.is_success);
        Assert.NotNull(result.data);
        Assert.Equal(expectedList.Data.Count, result.data.Data.Count);
    }

}


