﻿using Core.Enums;
using Core.Pagination;
using DataLayer.Base.Response;
using MediatR;

namespace DataLayer.Base.Query;
/// <summary>
/// کوئری عمومی برای دریافت لیست صفحه‌بندی شده از نوع <typeparamref name="T"/> با امکان جستجو و مرتب‌سازی.
/// </summary>
/// <typeparam name="T">نوع داده‌ای که قرار است صفحه‌بندی شود.</typeparam>
public class GetPaginatedQuery<T> : IRequest<ResponseDto<PaginatedList<T>>>
{
	/// <summary>
	/// عبارت جستجو برای فیلتر کردن داده‌ها (اختیاری).
	/// </summary>
	public string? SearchTerm { get; set; }

	/// <summary>
	/// معیار مرتب‌سازی داده‌ها. مقدار پیش‌فرض مرتب‌سازی بر اساس تاریخ ایجاد است.
	/// </summary>
	public SortByEnum SortBy { get; set; } = SortByEnum.created_at;

	/// <summary>
	/// تعداد آیتم‌ها در هر صفحه. مقدار پیش‌فرض 10 است.
	/// </summary>
	public int PageSize { get; set; } = 10;

	/// <summary>
	/// شماره صفحه جاری. مقدار پیش‌فرض 1 است.
	/// </summary>
	public int Page { get; set; } = 1;
}