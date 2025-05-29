using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Assistant.Helpers;
public static class SlugHelper
{
    public static string GenerateSlug(string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase))
            return string.Empty;

        // تبدیل به حروف کوچک و نرمال‌سازی
        string normalized = phrase.ToLowerInvariant().Normalize(NormalizationForm.FormD);

        // حذف علائم اعراب و لهجه‌ها (مثل é → e)
        var sb = new StringBuilder();
        foreach (char c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        string cleaned = sb.ToString();

        // حذف هر چیزی غیر از حروف، اعداد یا فاصله
        cleaned = Regex.Replace(cleaned, @"[^a-z0-9\s-]", "");

        // تبدیل فاصله‌ها و خط تیره‌های متوالی به یک خط تیره
        cleaned = Regex.Replace(cleaned, @"[\s-]+", "-").Trim('-');

        return cleaned;
    }
}