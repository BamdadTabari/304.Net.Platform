using DataLayer.Base.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Base.Query;
public class GetBySlugQuery<T> : IRequest<ResponseDto<T>>
{
    [Display(Name = "نامک")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public string slug { get; set; }
}
