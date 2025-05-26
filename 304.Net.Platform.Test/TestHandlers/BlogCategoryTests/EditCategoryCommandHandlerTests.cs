using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Test.DataProvider;
using _304.Net.Platform.Test.GenericHandlers;
using Core.EntityFramework.Models;
using DataLayer.Services;

namespace _304.Net.Platform.Test.TestHandlers.BlogCategoryTests;
public class EditCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEditCategory_WhenEntityExists()
    {
        await EditHandlerTestHelper.TestEditSuccess<EditCategoryCommand, 
            BlogCategory, 
            EditCategoryCommandHandler>(
            handlerFactory: (repo, uow) => new EditCategoryCommandHandler(uow, repo), // فقط IUnitOfWork پاس می‌دهیم
            execute: (handler, command, token) => handler.Handle(command, token),
            command: BlogCategoryDataProvider.Edit(name: "New", id: 1),
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
                Assert.Equal("updated-name", entity.slug); // فرض بر این است که SlugHelper ساخته
                Assert.Equal("Updated Desc", entity.description);
            }
        );
    }


    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenEntityDoesNotExist()
    {
        await EditHandlerTestHelper.TestEditNotFound<EditCategoryCommand, BlogCategory, EditCategoryCommandHandler>(
            handlerFactory: (repo, uow) => new EditCategoryCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: new EditCategoryCommand { id = 2 },
            entityId: 2
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnCommitFail_WhenCommitFails()
    {
        await EditHandlerTestHelper.TestEditCommitFail<EditCategoryCommand, BlogCategory, EditCategoryCommandHandler>(
            handlerFactory: (repo, uow) => new EditCategoryCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: new EditCategoryCommand
            {
                id = 1,
                name = "Name",
                slug = null,
                description = "Desc"
            },
            entityId: 1,
            existingEntity: new BlogCategory
            {
                id = 1,
                name = "Old Name",
                slug = "old-slug",
                description = "Old Desc"
            }
        );
    }
}