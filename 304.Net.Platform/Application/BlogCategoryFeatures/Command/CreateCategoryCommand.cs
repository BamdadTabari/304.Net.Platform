using DataLayer.Base.Command;
using System.ComponentModel.DataAnnotations;

namespace _304.Net.Platform.Application.BlogCategoryFeatures.Command;

public class CreateCategoryCommand : CreateCommand
{
	[Display(Name = "توضیحات")]
	public string? description { get; set; }
}
