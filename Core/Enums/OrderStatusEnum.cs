using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Enums;

public enum OrderStatusEnum
{
	[Display(Name = "در حال انجام")]
	InProgress = 0,

	[Display(Name = "موفقیت آمیز")]
	Success = 1,

	[Display(Name = "شکست خورده")]
	Fail = 2,
}
