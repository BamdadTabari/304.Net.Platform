using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using Core.EntityFramework.Models;
using Moq;
using System.Linq.Expressions;

namespace _304.Net.Platform.Test;
public class CreateCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateCategory_WhenNameAndSlugAreUnique()
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            name = "Test Category",
            slug = null,
            description = "Test Category Description"
        };

        await CreateHandlerTestHelper.TestCreateSuccess<
            CreateCategoryCommand,
            BlogCategory,
            CreateCategoryCommandHandler
        >(
            handlerFactory: unitOfWork => new CreateCategoryCommandHandler(unitOfWork),

            execute: (handler, command, token) => handler.Handle(command, token),

            command: command,

            repoSelector: uow => uow.BlogCategoryRepository,

            setupRepoMock: repoMock =>
            {
                repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<BlogCategory, bool>>>()))
                        .ReturnsAsync(false);

                repoMock.Setup(r => r.AddAsync(It.IsAny<BlogCategory>()))
                        .Returns(Task.CompletedTask);
            }
        );
    }
}
