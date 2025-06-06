﻿using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Test.DataProvider;
using _304.Net.Platform.Test.GenericHandlers;
using Core.EntityFramework.Models;
using DataLayer.Repository;
using DataLayer.Services;
using Moq;
using System.Linq.Expressions;

namespace _304.Net.Platform.Test.TestHandlers.BlogCategoryTests;
public class CreateCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateCategory_WhenNameAndSlugAreUnique()
    {
        await CreateHandlerTestHelper.TestCreateSuccess<
            CreateCategoryCommand,                 // Command Type
            BlogCategory,                          // Entity Type
            IBlogCategoryRepository,               // Repository Interface
            CreateCategoryCommandHandler           // Handler Type
        >(
            handlerFactory: uow => new CreateCategoryCommandHandler(uow),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: BlogCategoryDataProvider.Create(),
            repoSelector: uow => uow.BlogCategoryRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenNameIsDuplicate()
    {
        await CreateHandlerTestHelper.TestCreateFailure<
            CreateCategoryCommand,
            BlogCategory,
            IBlogCategoryRepository,
            CreateCategoryCommandHandler
        >(
            handlerFactory: uow => new CreateCategoryCommandHandler(uow),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: BlogCategoryDataProvider.Create(name: "Duplicated Name"),
            repoSelector: uow => uow.BlogCategoryRepository,
            setupRepoMock: repo =>
            {
                // name تکراری است => باید true برگرداند تا Valid نباشد
                repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<BlogCategory, bool>>>()))
                    .ReturnsAsync(true);
            },
            expectedMessage: null
        );
    }


    [Fact]
    public async Task Handle_ShouldFail_WhenSlugIsDuplicate()
    {
        await CreateHandlerTestHelper.TestCreateFailure<
            CreateCategoryCommand,
            BlogCategory,
            IBlogCategoryRepository,
            CreateCategoryCommandHandler
        >(
            handlerFactory: uow => new CreateCategoryCommandHandler(uow),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: new CreateCategoryCommand { name = "Test Name", slug = "duplicate-slug" },
            repoSelector: uow => uow.BlogCategoryRepository,
            setupRepoMock: repo =>
            {
                repo.SetupSequence(r => r.ExistsAsync(It.IsAny<Expression<Func<BlogCategory, bool>>>()))
                    .ReturnsAsync(false) // برای name → یعنی name تکراری نیست
                    .ReturnsAsync(true); // برای slug → یعنی slug تکراری است
            },
            expectedMessage: null
        );
    }



    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionThrown()
    {
        // Arrange
        var command = new CreateCategoryCommand { name = "Tech" };

        await CreateHandlerTestHelper.TestCreateException<
            CreateCategoryCommand,
            BlogCategory,
            IBlogCategoryRepository,
            CreateCategoryCommandHandler>(

            handlerFactory: uow => new CreateCategoryCommandHandler(uow),

            execute: (handler, cmd, token) => handler.Handle(cmd, token),

            command: command,

            repoSelector: uow => uow.BlogCategoryRepository,

            setupRepoMock: repoMock =>
            {
                // نام تکراری نیست
                repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<BlogCategory, bool>>>()))
                    .ReturnsAsync(false);

                // شبیه‌سازی Exception هنگام Add
                repoMock.Setup(r => r.AddAsync(It.IsAny<BlogCategory>()))
                    .ThrowsAsync(new Exception("DB error"));
            }
        );
    }


}
