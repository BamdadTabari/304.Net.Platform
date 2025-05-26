using DataLayer.Base.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Base.Command;
public class DeleteCommand : IRequest<ResponseDto<string>>
{
	[Display(Name = "آیدی")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public long id { get; set; }
}
