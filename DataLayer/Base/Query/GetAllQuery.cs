﻿using DataLayer.Base.Response;
using MediatR;

namespace DataLayer.Base.Query;
/// <summary>
/// کوئری عمومی برای دریافت تمامی رکوردها از نوع داده <typeparamref name="T"/>.
/// این کلاس از MediatR برای ارسال درخواست و دریافت پاسخ استفاده می‌کند.
/// </summary>
/// <typeparam name="T">نوع داده‌ای که قرار است لیست آن بازیابی شود.</typeparam>
public class GetAllQuery<T> : IRequest<ResponseDto<List<T>>>
{
}
