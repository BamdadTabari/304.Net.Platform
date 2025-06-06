﻿using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogFeatures.Command;
using _304.Net.Platform.Application.BlogFeatures.Query;
using _304.Net.Platform.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _304.Net.Platform.Controllers;
[Route("api/blog")]
[ApiController]
public class BlogController(IMediator mediator) : BaseController
{
	private readonly IMediator _mediator = mediator;

	[HttpGet]
	[Route("list")]
	public async Task<IActionResult> Index(GetPaginatedCategoryQuery query, CancellationToken cancellationToken)
	{
		try
		{
			var result = await _mediator.Send(query, cancellationToken);
			return Ok(result);
		}
		catch (Exception ex)
		{
			return HandleException(ex);
		}
	}

	[HttpGet]
	[Route("all")]
	public async Task<IActionResult> GetAll(GetAllCategoryQuery query, CancellationToken cancellationToken)
	{
		try
		{
			var result = await _mediator.Send(query, cancellationToken);
			return Ok(result);
		}
		catch (Exception ex)
		{
			return HandleException(ex);
		}
	}

	[HttpPost]
	[Route("create")]
	public async Task<IActionResult> Create([FromForm] CreateCategoryCommand command, CancellationToken cancellationToken)
	{
		try
		{
			if (!ModelState.IsValid)
				return InvalidModelResponse();
			var response = await _mediator.Send(command, cancellationToken);
			return Ok(response);
		}
		catch (Exception ex)
		{
			return HandleException(ex);
		}
	}

	[HttpPost]
	[Route("edit")]
	public async Task<IActionResult> Edit([FromForm] EditCategoryCommand command, CancellationToken cancellationToken)
	{
		try
		{
			if (!ModelState.IsValid)
				return InvalidModelResponse();
			var response = await _mediator.Send(command, cancellationToken);
			return Ok(response);
		}
		catch (Exception ex)
		{
			return HandleException(ex);
		}
	}

	[HttpGet]
	[Route("get")]
	public async Task<IActionResult> GetBySlug([FromForm] GetBlogBySlugQuery query, CancellationToken cancellationToken)
	{
		try
		{
			if (!ModelState.IsValid)
				return InvalidModelResponse();
			var response = await _mediator.Send(query, cancellationToken);
			return Ok(response);
		}
		catch (Exception ex)
		{
			return HandleException(ex);
		}
	}

	[HttpPost]
	[Route("delete")]
	public async Task<IActionResult> Delete([FromForm] DeleteBlogCommand command, CancellationToken cancellationToken)
	{
		try
		{
			if (!ModelState.IsValid)
				return InvalidModelResponse();
			var response = await _mediator.Send(command, cancellationToken);
			return Ok(response);
		}
		catch (Exception ex)
		{
			return HandleException(ex);
		}
	}
}