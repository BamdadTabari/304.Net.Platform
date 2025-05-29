using Core.Base.EF;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _304.Net.Platform.Test.GenericHandlers;
public static class EditHandlerTestHelper
{
    public static async Task TestEditSuccess<TCommand, TEntity, THandler>(
        Func<IRepository<TEntity>, IUnitOfWork, THandler> handlerFactory,
        Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
        TCommand command,
        long entityId,
        TEntity existingEntity,
        Action<TEntity> assertUpdated
    )
        where TEntity : class, IBaseEntity
        where THandler : class
    {
        // Arrange
        var repoMock = new Mock<IRepository<TEntity>>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        // موجودیت فعلی که باید ویرایش بشه
        repoMock.Setup(r => r.FindSingle(x => x.id == entityId))
                .ReturnsAsync(existingEntity);

        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        // ساخت هندلر با mockها
        var handler = handlerFactory(repoMock.Object, unitOfWorkMock.Object);

        // Act
        var result = await execute(handler, command, CancellationToken.None);

        // Assert
        Assert.True(result.is_success);
        Assert.Equal(200, result.response_code);
        Assert.NotNull(result.data);

        // بررسی تغییرات روی موجودیت
        assertUpdated(existingEntity);

        repoMock.Verify(r => r.FindSingle(x => x.id == entityId), Times.Once);
        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    // ✴️ حالت: موجودیت یافت نشد
    public static async Task TestEditNotFound<TCommand, TEntity, THandler>(
        Func<IRepository<TEntity>, IUnitOfWork, THandler> handlerFactory,
        Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
        TCommand command,
        long entityId
    )
        where TEntity : class, IBaseEntity
        where THandler : class
    {
        var repoMock = new Mock<IRepository<TEntity>>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        repoMock.Setup(r => r.FindSingle(x => x.id == entityId))
                .ReturnsAsync((TEntity?)null);

        var handler = handlerFactory(repoMock.Object, unitOfWorkMock.Object);
        var result = await execute(handler, command, CancellationToken.None);

        Assert.False(result.is_success);
        Assert.Equal(404, result.response_code);

        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }


    // ✴️ حالت: Commit با شکست مواجه می‌شود
    public static async Task TestEditCommitFail<TCommand, TEntity, THandler>(
        Func<IRepository<TEntity>, IUnitOfWork, THandler> handlerFactory,
        Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
        TCommand command,
        long entityId,
        TEntity existingEntity
    )
        where TEntity : class, IBaseEntity
        where THandler : class
    {
        var repoMock = new Mock<IRepository<TEntity>>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        repoMock.Setup(r => r.FindSingle(x => x.id == entityId))
                .ReturnsAsync(existingEntity);

        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(false); // Fail commit

        var handler = handlerFactory(repoMock.Object, unitOfWorkMock.Object);
        var result = await execute(handler, command, CancellationToken.None);

        Assert.False(result.is_success);
        Assert.Equal(500, result.response_code);

        repoMock.Verify(r => r.FindSingle(x => x.id == entityId), Times.Once);
        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

