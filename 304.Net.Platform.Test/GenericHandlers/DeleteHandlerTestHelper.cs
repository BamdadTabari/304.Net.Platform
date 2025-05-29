using Core.Base.EF;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _304.Net.Platform.Test.GenericHandlers;
public static class DeleteHandlerTestHelper
{
    public static async Task TestDelete<TCommand, TEntity, TRepository, THandler>(
    Func<IUnitOfWork, THandler> handlerFactory,
    Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
    TCommand command,
    Expression<Func<IUnitOfWork, TRepository>> repoSelector,
    Action<Mock<TRepository>>? setupRepoMock = null
)
    where TEntity : class, IBaseEntity
    where TRepository : class, IRepository<TEntity>
    where THandler : class
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

        var entity = Mock.Of<TEntity>();
        repoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync(entity);
        repoMock.Setup(r => r.Remove(It.IsAny<TEntity>()));
        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        setupRepoMock?.Invoke(repoMock);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, command, CancellationToken.None);

        Assert.True(result.is_success);
        Assert.Equal(204, result.response_code);

        repoMock.Verify(r => r.Remove(It.IsAny<TEntity>()), Times.Once);
        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    public static async Task TestDeleteNotFound<TCommand, TEntity, TRepository, THandler>(
    Func<IUnitOfWork, THandler> handlerFactory,
    Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
    TCommand command,
    Expression<Func<IUnitOfWork, TRepository>> repoSelector
)
    where TEntity : class, IBaseEntity
    where TRepository : class, IRepository<TEntity>
    where THandler : class
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);
        repoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync((TEntity?)null);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, command, CancellationToken.None);

        Assert.False(result.is_success);
        Assert.Equal(404, result.response_code);
    }
}
