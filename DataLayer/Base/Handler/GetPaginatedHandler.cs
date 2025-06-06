﻿using Core.Pagination;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Serilog;

namespace DataLayer.Base.Handler;
/// <summary>
/// هندلر عمومی برای دریافت داده‌های صفحه‌بندی‌شده (Paginated) از ریپازیتوری با استفاده از UnitOfWork.
/// </summary>
public class GetPaginatedHandler
{
	private readonly IUnitOfWork _unitOfWork;

	/// <summary>
	/// سازنده کلاس که وابستگی به <see cref="IUnitOfWork"/> را دریافت می‌کند.
	/// </summary>
	/// <param name="unitOfWork">واحد کاری برای دسترسی به داده‌ها</param>
	public GetPaginatedHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	/// <summary>
	/// دریافت داده‌های صفحه‌بندی شده از ریپازیتوری.
	/// </summary>
	/// <typeparam name="TEntity">نوع موجودیت (Entity) داده‌ها</typeparam>
	/// <param name="getRepo">تابعی که عملیات واکشی داده‌های صفحه‌بندی شده را با UnitOfWork انجام می‌دهد</param>
	/// <returns>
	/// شیء <see cref="ResponseDto{PaginatedList}"/> شامل لیست صفحه‌بندی‌شده داده‌ها یا پیام خطا
	/// </returns>
	public async Task<ResponseDto<PaginatedList<TEntity>>> Handle<TEntity>(
		Func<IUnitOfWork, Task<PaginatedList<TEntity>>> getRepo)
		where TEntity : class
	{
		try
		{
			var result = await getRepo(_unitOfWork);
			return Responses.Data(result);
		}
		catch (Exception ex)
		{
			// لاگ‌گیری مستقیم با Serilog
			Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
			return Responses.ExceptionFail<PaginatedList<TEntity>>(default, null);
		}
	}
}
