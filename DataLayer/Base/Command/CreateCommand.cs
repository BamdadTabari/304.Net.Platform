using DataLayer.Base.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Base.Command;
public class CreateCommand : IRequest<ResponseDto<string>>
{
	[Display(Name = "نامک")]
	public string? slug { get; set; }

	[Display(Name = "نام")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public string name { get; set; }

	[Display(Name = "زمان ایجاد")]
	public DateTime created_at { get; set; } = DateTime.Now;
	[Display(Name = "زمان ویرایش")]
	public DateTime updated_at { get; set; } = DateTime.Now;
}