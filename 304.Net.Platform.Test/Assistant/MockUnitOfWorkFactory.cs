using DataLayer.Repository;
using DataLayer.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace _304.Net.Platform.Test.Assistant;

// todo
public static class MockUnitOfWorkFactory
{
	public static Mock<IUnitOfWork> Create(
		Mock<IBlogRepository>? blogRepositoryMock = null,
		Mock<IBlogCategoryRepository>? blogCategoryRepositoryMock = null)
	{
		var unitOfWorkMock = new Mock<IUnitOfWork>();

		// اگر موک ریپازیتوری ارسال نشده بود، یکی بساز
		blogRepositoryMock ??= new Mock<IBlogRepository>();
		blogCategoryRepositoryMock ??= new Mock<IBlogCategoryRepository>();

		unitOfWorkMock.Setup(u => u.BlogRepository).Returns(blogRepositoryMock.Object);
		unitOfWorkMock.Setup(u => u.BlogCategoryRepository).Returns(blogCategoryRepositoryMock.Object);

		// اگر خواستی متد CommitAsync هم تنظیم کن
		unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
					  .ReturnsAsync(true);

		return unitOfWorkMock;
	}
}