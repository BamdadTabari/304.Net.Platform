using Core.Base.Text;

namespace DataLayer.Base.Response;

public static class Responses
{
    public static ResponseDto<T> Success<T>(T? data = default, string? message = null, int code = 200)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = true,
            message = message ?? Messages.Success(),
            response_code = code
        };
    }

    public static ResponseDto<T> ChangeOrDelete<T>(T? data = default, string? message = null, int code = 204)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = true,
            message = message ?? Messages.Success(),
            response_code = code
        };
    }

    public static ResponseDto<T> Fail<T>(T? data = default, string? message = null, int code = 500)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = false,
            message = message ?? Messages.Fail(),
            response_code = code
        };
    }

    public static ResponseDto<T> Exist<T>(T? data, string? name, string property, string? message = null)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = false,
            message = message ?? Messages.Exist(name, property),
            response_code = 409 // Conflict
        };
    }

    public static ResponseDto<T> NotFound<T>(T? data, string? name = null, string? message = null)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = false,
            message = message ?? Messages.NotFound(name),
            response_code = 404
        };
    }

    public static ResponseDto<T> Data<T>(T data, string? message = null, int code = 200)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = true,
            message = message ?? "عملیات موفق",
            response_code = code
        };
    }
}