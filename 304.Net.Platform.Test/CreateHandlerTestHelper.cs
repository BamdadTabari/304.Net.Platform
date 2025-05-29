using Core.Base.EF;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Moq;
using System.Linq.Expressions;

namespace _304.Net.Platform.Test;
public static class CreateHandlerTestHelper
{
    public static async Task TestCreateSuccess<TCommand, TEntity, THandler>(
        Func<IUnitOfWork, THandler> handlerFactory,
        Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
        TCommand command,
        Expression<Func<IUnitOfWork, IRepository<TEntity>>> repoSelector,
        Action<Mock<IRepository<TEntity>>>? setupRepoMock = null
    ) where TEntity : class, IBaseEntity        // 🔸 این خط مهمه!
    where THandler : class
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<IRepository<TEntity>>();

        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

        // پیش‌فرض‌ها
        repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync(false);
        repoMock.Setup(r => r.AddAsync(It.IsAny<TEntity>())).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // تنظیمات اختیاری
        setupRepoMock?.Invoke(repoMock);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, command, CancellationToken.None);

        // Assert
        Assert.True(result.is_success);
        Assert.Equal(201, result.response_code);
        Assert.NotNull(result.data);

        repoMock.Verify(r => r.AddAsync(It.IsAny<TEntity>()), Times.Once);
        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
