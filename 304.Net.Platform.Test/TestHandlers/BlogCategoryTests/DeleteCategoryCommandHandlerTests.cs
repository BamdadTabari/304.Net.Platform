using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Test.GenericHandlers;
using Core.EntityFramework.Models;
using DataLayer.Services;

namespace _304.Net.Platform.Test.TestHandlers.BlogCategoryTests;
public class DeleteCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteCategory_WhenExists()
    {
        var command = new DeleteCategoryCommand { id = 1 };

        await DeleteHandlerTestHelper.TestDelete<
            DeleteCategoryCommand,
            BlogCategory,
            IBlogCategoryRepository,
            DeleteCategoryCommandHandler>(
            handlerFactory: uow => new DeleteCategoryCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.BlogCategoryRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        var command = new DeleteCategoryCommand { id = 99 };

        await DeleteHandlerTestHelper.TestDeleteNotFound<
            DeleteCategoryCommand,
            BlogCategory,
            IBlogCategoryRepository,
            DeleteCategoryCommandHandler>(
            handlerFactory: uow => new DeleteCategoryCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.BlogCategoryRepository
        );
    }

}
