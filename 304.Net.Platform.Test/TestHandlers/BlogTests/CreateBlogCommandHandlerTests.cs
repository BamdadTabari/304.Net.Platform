﻿using _304.Net.Platform.Application.BlogFeatures.Command;
using _304.Net.Platform.Application.BlogFeatures.Handler;
using _304.Net.Platform.Application.BlogFeatures.Command;
using _304.Net.Platform.Application.BlogFeatures.Handler;
using _304.Net.Platform.Test.GenericHandlers;
using Core.EntityFramework.Models;
using DataLayer.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Http;
using Core.Assistant.Helpers;
using _304.Net.Platform.Test.Assistant;

namespace _304.Net.Platform.Test.TestHandlers.BlogTests;
public class CreateBlogCommandHandlerTests
{
   
    [Fact]
    public async Task Handle_ShouldCreateBlog_WhenNameAndSlugAreUnique()
    {
        var file = Files.CreateFakeFormFile();
        await CreateHandlerTestHelper.TestCreateSuccess<
            CreateBlogCommand,                 // Command Type
            Blog,                          // Entity Type
            IBlogRepository,               // Repository Interface
            CreateBlogCommandHandler           // Handler Type
        >(
            handlerFactory: uow => new CreateBlogCommandHandler(uow),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: new CreateBlogCommand 
            { name = "Test Blog", slug = null , blog_category_id = 1, image_file = file,
            image_alt = "" },
            repoSelector: uow => uow.BlogRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenNameIsDuplicate()
    {
		var file = Files.CreateFakeFormFile();
		await CreateHandlerTestHelper.TestCreateFailure<
            CreateBlogCommand,
            Blog,
            IBlogRepository,
            CreateBlogCommandHandler
        >(
            handlerFactory: uow => new CreateBlogCommandHandler(uow),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: new CreateBlogCommand { name = "Duplicate Name", slug = null , image_file =file },
            repoSelector: uow => uow.BlogRepository,
            setupRepoMock: repo =>
            {
                // name تکراری است => باید true برگرداند تا Valid نباشد
                repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Blog, bool>>>()))
                    .ReturnsAsync(true);
            },
            expectedMessage: null
        );
    }


    [Fact]
    public async Task Handle_ShouldFail_WhenSlugIsDuplicate()
    {
		var file = Files.CreateFakeFormFile();
		await CreateHandlerTestHelper.TestCreateFailure<
            CreateBlogCommand,
            Blog,
            IBlogRepository,
            CreateBlogCommandHandler
        >(
            handlerFactory: uow => new CreateBlogCommandHandler(uow),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: new CreateBlogCommand { name = "Test Name", slug = "duplicate-slug" , image_file = file},
            repoSelector: uow => uow.BlogRepository,
            setupRepoMock: repo =>
            {
                repo.SetupSequence(r => r.ExistsAsync(It.IsAny<Expression<Func<Blog, bool>>>()))
                    .ReturnsAsync(false) // برای name → یعنی name تکراری نیست
                    .ReturnsAsync(true); // برای slug → یعنی slug تکراری است
            },
            expectedMessage: null
        );
    }



    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionThrown()
    {
		var file = Files.CreateFakeFormFile();
		// Arrange
		var command = new CreateBlogCommand { name = "Tech" , image_file = file};
		
		await CreateHandlerTestHelper.TestCreateException<
            CreateBlogCommand,
            Blog,
            IBlogRepository,
            CreateBlogCommandHandler>(

            handlerFactory: uow => new CreateBlogCommandHandler(uow),

            execute: (handler, cmd, token) => handler.Handle(cmd, token),

            command: command,

            repoSelector: uow => uow.BlogRepository,

            setupRepoMock: repoMock =>
            {
                // نام تکراری نیست
                repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Blog, bool>>>()))
                    .ReturnsAsync(false);

                // شبیه‌سازی Exception هنگام Add
                repoMock.Setup(r => r.AddAsync(It.IsAny<Blog>()))
                    .ThrowsAsync(new Exception("DB error"));
            }
        );
    }

	[Fact]
	public async Task Handle_ShouldFail_WhenImageFileIsNull()
	{
		await CreateHandlerTestHelper.TestCreateFailure<
			CreateBlogCommand,
			Blog,
			IBlogRepository,
			CreateBlogCommandHandler
		>(
			handlerFactory: uow => new CreateBlogCommandHandler(uow),

			execute: (handler, cmd, ct) => handler.Handle(cmd, ct),

			command: new CreateBlogCommand
			{
				name = "Blog Without Image",
				slug = null,
				image_file = null // 🔴 عمداً فایل نذاشتیم
			},

			repoSelector: uow => uow.BlogRepository,

			setupRepoMock: repo =>
			{
				// حتی نیاز نیست چیزی ستاپ کنیم چون کد قبل از رسیدن به validation ها return می‌کنه
			},

			expectedMessage: "تصویر شاخص"
		);
	}
}
