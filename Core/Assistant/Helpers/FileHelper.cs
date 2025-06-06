﻿using Azure.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Assistant.Helpers;

/// <summary>
/// کلاس کمکی برای مدیریت فایل‌ها (آپلود و حذف تصاویر)
/// </summary>
public static class FileHelper
{
	/// <summary>
	/// بارگذاری (آپلود) تصویر به مسیر مشخص‌شده در پروژه
	/// </summary>
	/// <param name="file">فایل ارسالی از سمت کلاینت (فرم)</param>
	/// <param name="folderName">نام پوشه‌ای که تصویر در آن ذخیره می‌شود (پیش‌فرض: images)</param>
	/// <returns>مسیر فیزیکی کامل فایل ذخیره‌شده روی سرور</returns>
	public static async Task<string> UploadImage(IFormFile file, string folderName = "images")
	{
		// مسیر کامل پوشه ذخیره‌سازی بر اساس مسیر فعلی پروژه
		var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

		// در صورت نبودن پوشه، آن را ایجاد می‌کنیم
		if (!Directory.Exists(uploadPath))
		{
			Directory.CreateDirectory(uploadPath);
		}

		// ساخت نام جدید یکتا برای فایل با استفاده از GUID و پسوند فایل اصلی
		var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
		var imagePath = Path.Combine(uploadPath, newFileName);

		// ذخیره‌سازی فایل در مسیر مشخص‌شده
		using (var stream = new FileStream(imagePath, FileMode.Create))
		{
			await file.CopyToAsync(stream);
		}

		return imagePath; // بازگرداندن مسیر فیزیکی تصویر ذخیره‌شده
	}

	/// <summary>
	/// حذف فایل تصویر از مسیر مشخص
	/// </summary>
	/// <param name="imagePath">مسیر کامل فایل تصویری که باید حذف شود</param>
	public static void DeleteImage(string imagePath)
	{
		// اگر فایل وجود داشته باشد، آن را حذف می‌کنیم
		if (File.Exists(imagePath))
			File.Delete(imagePath);
	}
}
