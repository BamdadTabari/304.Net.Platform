using DataLayer.Base.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Base.Command;
public class EditCommand : IRequest<ResponseDto<string>>
{
	[Display(Name = "آیدی")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public long id { get; set; }

	[Display(Name = "نامک")]
	public string? slug { get; set; }

	[Display(Name = "نام")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public string name { get; set; }

	[Display(Name = "زمان ویرایش")]
	public DateTime updated_at { get; set; } = DateTime.Now;
}
