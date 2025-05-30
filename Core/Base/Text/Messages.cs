namespace Core.Base.Text;
public static class Messages
{
    public static string ExceptionFail() => "خطایی غیر منتظره رخ داد لطفا دوباره تلاش کنید";
    public static string Fail() => "عملیات ناموفق بود";

    public static string Success() => "عملیات موفق بود";

    public static string NotFound(string? name) =>
        !string.IsNullOrWhiteSpace(name) ? $"{name} پیدا نشد" : "آیتم پیدا نشد";

    public static string Exist(string? name, string property) =>
        !string.IsNullOrWhiteSpace(name) ? $"{name} با این {property} وجود دارد" : $"آیتم با این {property} وجود دارد";

    public static string ChangeStatus(string name) => $"وضعیت {name} تغییر کرد";

    public static string Validate(string name) => $"مقدار {name} را وارد کنید";
}