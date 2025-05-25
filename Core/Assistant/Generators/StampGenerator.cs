using Core.Base.Text;

namespace Core.Assistant.Generators;

public static class StampGenerator
{
	public static string CreateSecurityStamp(int length)
	{
		if (length <= 0)
			throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than zero.");

		return RandomGenerator
			.GenerateString(length, AllowedCharacters.Alphanumeric)
			.ToUpperInvariant();
	}
}