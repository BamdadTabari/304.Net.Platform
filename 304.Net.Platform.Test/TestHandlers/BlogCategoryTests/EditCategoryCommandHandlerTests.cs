using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Test.GenericHandlers;
using Core.EntityFramework.Models;

namespace _304.Net.Platform.Test.TestHandlers.BlogCategoryTests;
public class EditCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEditCategory_WhenEntityExists()
    {
        await EditHandlerTestHelper.TestEditSuccess<EditCategoryCommand, BlogCategory, EditCategoryCommandHandler>(
             handlerFactory: (repo, uow) => new EditCategoryCommandHandler(repo, uow),
             execute: (handler, command, token) => handler.Handle(command, token),
             command: new EditCategoryCommand
             {
                 id = 1,
                 name = "Updated Name",
                 slug = null,
                 description = "Updated Desc"
             },
             entityId: 1,
            existingEntity: new BlogCategory
            {
                id = 1,
                name = "Old Name",
                slug = "old-name",
                description = "Old Desc"
            },
            assertUpdated: entity =>
            {
                Assert.Equal("Updated Name", entity.name);
                Assert.Equal("updated-name", entity.slug);
                Assert.Equal("Updated Desc", entity.description);
            }
        );

    }


    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenEntityDoesNotExist()
    {
        await EditHandlerTestHelper.TestEditNotFound<EditCategoryCommand, BlogCategory, EditCategoryCommandHandler>(
            handlerFactory: (repo, uow) => new EditCategoryCommandHandler(repo, uow),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: new EditCategoryCommand { id = 99 },
            entityId: 99
        );
    }


    [Fact]
    public async Task Handle_ShouldReturnServerError_WhenCommitFails()
    {
        await EditHandlerTestHelper.TestEditCommitFail<EditCategoryCommand, BlogCategory, EditCategoryCommandHandler>(
            handlerFactory: (repo, uow) => new EditCategoryCommandHandler(repo, uow),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: new EditCategoryCommand
            {
                id = 1,
                name = "Test"
            },
            entityId: 1,
            existingEntity: new BlogCategory { id = 1, name = "Old" }
        );
    }


    [Fact]
    public async Task Handle_ShouldUseProvidedSlug_WhenSlugIsProvided()
    {
        await EditHandlerTestHelper.TestEditSuccess<EditCategoryCommand, BlogCategory, EditCategoryCommandHandler>(
            handlerFactory: (repo, uow) => new EditCategoryCommandHandler(repo, uow),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: new EditCategoryCommand
            {
                id = 1,
                name = "Title",
                slug = "custom-slug"
            },
            entityId: 1,
            existingEntity: new BlogCategory { id = 1, name = "Old", slug = "old" },
            assertUpdated: entity =>
            {
                Assert.Equal("custom-slug", entity.slug);
            }
        );
    }

}
