﻿using DataLayer.Base.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Base.Command;
/// <summary>
/// فرمان ایجاد (CreateCommand) برای ایجاد یک موجودیت جدید.
/// این فرمان از MediatR استفاده می‌کند و به عنوان درخواست (Request)
/// برای هندلر مرتبط جهت انجام عملیات ذخیره‌سازی استفاده می‌شود.
/// </summary>
/// <remarks>
/// این کلاس به صورت عمومی قابل استفاده برای انواع موجودیت‌های ساده‌ای است
/// که دارای فیلدهای مشترکی مانند نام (name)، نامک (slug) و زمان‌های ایجاد و ویرایش هستند.
/// </remarks>
public class CreateCommand : IRequest<ResponseDto<string>>
{
	/// <summary>
	/// نامک (slug) برای ایجاد آدرس‌های یکتا و خوانا در URLها استفاده می‌شود.
	/// اختیاری است.
	/// </summary>
	[Display(Name = "نامک")]
	public string? slug { get; set; }

	/// <summary>
	/// نام موجودیت. مقدار الزامی که نباید خالی باشد.
	/// </summary>
	[Display(Name = "نام")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public string name { get; set; }

	/// <summary>
	/// زمان ایجاد رکورد. به صورت پیش‌فرض برابر با زمان فعلی است.
	/// </summary>
	[Display(Name = "زمان ایجاد")]
	public DateTime created_at { get; set; } = DateTime.Now;

	/// <summary>
	/// زمان آخرین ویرایش رکورد. به صورت پیش‌فرض برابر با زمان فعلی است.
	/// </summary>
	[Display(Name = "زمان ویرایش")]
	public DateTime updated_at { get; set; } = DateTime.Now;
}
