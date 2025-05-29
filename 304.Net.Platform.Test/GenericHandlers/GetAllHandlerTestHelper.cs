using Core.Base.EF;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _304.Net.Platform.Test.GenericHandlers;
public static class GetAllHandlerTestHelper
{
    public static async Task TestHandle_Success<TEntity, TDto, TRepository, THandler>(
       Func<IUnitOfWork, THandler> handlerFactory,
       Func<THandler, CancellationToken, Task<ResponseDto<List<TDto>>>> execute,
       Expression<Func<IUnitOfWork, TRepository>> repoSelector,
       List<TEntity> entities,
       Action<Mock<TRepository>>? setupRepoMock = null
   )
       where TEntity : class, IBaseEntity
       where TDto : class, new()
       where TRepository : class, IRepository<TEntity>
       where THandler : class
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

        // مهم: اینجا باید IQueryable برگردانده شود
        repoMock.Setup(r => r.FindList()).Returns(entities);

        setupRepoMock?.Invoke(repoMock);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, CancellationToken.None);

        Assert.True(result.is_success);
        Assert.NotNull(result.data);
    }

    public static async Task TestHandle_FailOnException<TEntity, TDto, TRepository, THandler>(
        Func<IUnitOfWork, THandler> handlerFactory,
        Func<THandler, CancellationToken, Task<ResponseDto<List<TDto>>>> execute,
        Expression<Func<IUnitOfWork, TRepository>> repoSelector,
        Action<Mock<TRepository>>? setupRepoMock = null)
        where TEntity : class, IBaseEntity
        where TDto : class, new()
        where TRepository : class, IRepository<TEntity>
        where THandler : class
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        // Mock repo to throw exception
        repoMock.Setup(r => r.FindList(It.IsAny<Expression<Func<TEntity, bool>>>())).Throws(new Exception("Test exception"));
        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

        setupRepoMock?.Invoke(repoMock);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, CancellationToken.None);

        Assert.False(result.is_success);
        Assert.Equal(500, result.response_code);
        //Assert.Contains("Test exception", result.message);
    }
}
