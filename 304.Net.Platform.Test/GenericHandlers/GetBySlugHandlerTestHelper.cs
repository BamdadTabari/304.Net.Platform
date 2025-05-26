using Core.Base.EF;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _304.Net.Platform.Test.GenericHandlers;
public static class GetBySlugHandlerTestHelper
{
	public static async Task TestGetBySlug_Success<TEntity, TDto, TRepository, THandler>(
	Func<IUnitOfWork, THandler> handlerFactory,
	Func<THandler, CancellationToken, Task<ResponseDto<TDto>>> execute,
	Expression<Func<IUnitOfWork, TRepository>> repoSelector,
	TEntity entity,
	string[]? includes = null
)
	where TEntity : class, IBaseEntity
	where TDto : class, new()
	where TRepository : class, IRepository<TEntity>
	where THandler : class
	{
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var repoMock = new Mock<TRepository>();

		unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

		var includesToUse = includes ?? Array.Empty<string>();

		repoMock.Setup(r =>
			r.FindSingle(
				It.IsAny<Expression<Func<TEntity, bool>>>(),
				It.Is<string[]>(inc => inc.SequenceEqual(includesToUse))
			)
		).ReturnsAsync(entity);

		var handler = handlerFactory(unitOfWorkMock.Object);
		var result = await execute(handler, CancellationToken.None);

		Assert.True(result.is_success);
		Assert.NotNull(result.data);
	}



	public static async Task TestGetBySlug_NotFound<TEntity, TDto, TRepository, THandler>(
        Func<IUnitOfWork, THandler> handlerFactory,
        Func<THandler, CancellationToken, Task<ResponseDto<TDto>>> execute,
        Expression<Func<IUnitOfWork, TRepository>> repoSelector
    )
        where TEntity : class, IBaseEntity
        where TDto : class, new()
        where TRepository : class, IRepository<TEntity>
        where THandler : class
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);
        repoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync((TEntity?)null);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, CancellationToken.None);

        Assert.False(result.is_success);
        Assert.Null(result.data);
    }
}
