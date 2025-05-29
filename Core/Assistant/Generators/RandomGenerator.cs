using System.Security.Cryptography;
using System.Text;

namespace Core.Assistant.Generators;
public static class RandomGenerator
{
    private static readonly char[] DefaultDigits = "0123456789".ToCharArray();

    public static string GenerateString(int length, string allowedCharacters, string prefix = "")
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
        if (string.IsNullOrEmpty(allowedCharacters)) throw new ArgumentException("allowedCharacters must not be empty.");

        var result = new StringBuilder(length);
        var buffer = new byte[sizeof(uint)];

        using var rng = RandomNumberGenerator.Create();
        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(buffer);
            var num = BitConverter.ToUInt32(buffer, 0);
            result.Append(allowedCharacters[(int)(num % allowedCharacters.Length)]);
        }

        return prefix + result;
    }

    public static int GenerateNumber(int max, int min = 0)
    {
        if (max <= min) throw new ArgumentOutOfRangeException(nameof(max), "max must be greater than min.");
        return RandomNumberGenerator.GetInt32(min, max);
    }

    public static string GenerateCode(int length = 8)
    {
        return GenerateString(length, new string(DefaultDigits));
    }
}