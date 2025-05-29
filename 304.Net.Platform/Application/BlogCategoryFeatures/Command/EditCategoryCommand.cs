using DataLayer.Base.Command;
using System.ComponentModel.DataAnnotations;

namespace _304.Net.Platform.Application.BlogCategoryFeatures.Command;

public class EditCategoryCommand : EditCommand
{
    [Display(Name = "توضیحات")]
    public string? description { get; set; }
}
