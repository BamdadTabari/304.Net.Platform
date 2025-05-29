using System.Security.Cryptography;

namespace Core.Assistant.Generators;
public static class PasswordHasher
{
    private const int SaltSize = 16; // 128 bit
    private const int KeySize = 32;  // 256 bit
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    public static string Hash(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithm);
        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{key}.{salt}";
    }

    public static bool Check(string hash, string password)
    {
        if (string.IsNullOrWhiteSpace(hash)) return false;

        var parts = hash.Split('.', 2);
        if (parts.Length != 2)
            throw new FormatException("Unexpected hash format. Should be formatted as 'key.salt'");

        var key = Convert.FromBase64String(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);

        using var algorithm = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm);
        var keyToCheck = algorithm.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(key, keyToCheck);
    }
}