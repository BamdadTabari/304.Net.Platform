using Core.Base.EF;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Moq;
using System.Linq.Expressions;

namespace _304.Net.Platform.Test.GenericHandlers;
public static class CreateHandlerTestHelper
{
    public static async Task TestCreateSuccess<TCommand, TEntity, TRepository, THandler>(
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

        repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync(false);
        repoMock.Setup(r => r.AddAsync(It.IsAny<TEntity>())).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        setupRepoMock?.Invoke(repoMock);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, command, CancellationToken.None);

        Assert.True(result.is_success);
        Assert.Equal(201, result.response_code);
        Assert.NotNull(result.data);

        repoMock.Verify(r => r.AddAsync(It.IsAny<TEntity>()), Times.Once);
        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    public static async Task TestCreateFailure<TCommand, TEntity, TRepository, THandler>(
    Func<IUnitOfWork, THandler> handlerFactory,
    Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
    TCommand command,
    Expression<Func<IUnitOfWork, TRepository>> repoSelector,
    Action<Mock<TRepository>>? setupRepoMock = null,
    string? expectedMessage = null
)
    where TEntity : class, IBaseEntity
    where TRepository : class, IRepository<TEntity>
    where THandler : class
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

        // مقداردهی پیش‌فرض: وجود نداشتن مورد مشابه (false) اگر نیاز داشتید تغییر دهید در setupRepoMock
        repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync(false);

        // چون قرار نیست اضافه کنیم یا Commit کنیم، انتظار نمی‌رود این‌ها صدا زده شوند ولی اگر خواستید Assert بگذارید
        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        setupRepoMock?.Invoke(repoMock);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, command, CancellationToken.None);

        Assert.False(result.is_success);
        Assert.NotEqual(201, result.response_code);
        if (!string.IsNullOrEmpty(expectedMessage))
        {
            Assert.Contains(expectedMessage, result.message ?? "", StringComparison.OrdinalIgnoreCase);
        }

        // معمولا در حالت خطا AddAsync و CommitAsync نباید فراخوانی شوند
        repoMock.Verify(r => r.AddAsync(It.IsAny<TEntity>()), Times.Never);
        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    public static async Task TestCreateException<TCommand, TEntity, TRepository, THandler>(
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

        setupRepoMock?.Invoke(repoMock);

        var handler = handlerFactory(unitOfWorkMock.Object);

        var result = await execute(handler, command, CancellationToken.None);

        Assert.False(result.is_success);
        Assert.Equal(500, result.response_code);  // فرض بر این است که Exception باعث کد 500 شود
        Assert.NotNull(result.message);

        // معمولا در این حالت AddAsync یا CommitAsync ممکن است فراخوانی نشوند یا بسته به ساختار متفاوت باشد
    }

}
