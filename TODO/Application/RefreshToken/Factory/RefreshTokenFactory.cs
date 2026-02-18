using System.Security.Cryptography;
using System.Text;

namespace TODO.Application.Refresh;

public class RefreshTokenFactory : IRefreshTokenFactory
{
	private const int _tokenDayLifeTime = 20;

	public RefreshTokenDto Create()
	{
		var rawToken = GenerateRaw();

		return new RefreshTokenDto(
			rawToken,
			GenerateHash(rawToken),
			DateTime.UtcNow,
			DateTime.UtcNow.AddDays(_tokenDayLifeTime));
	}

	public RefreshTokenDto Create(string rawToken)
	{
		return new RefreshTokenDto(
			rawToken,
			GenerateHash(rawToken),
			DateTime.UtcNow,
			DateTime.UtcNow.AddDays(_tokenDayLifeTime));
	}

	public bool Validate(string rawToken, string hashToken)
	{
		return CryptographicOperations.FixedTimeEquals(
			Convert.FromBase64String(hashToken),
			Convert.FromBase64String(GenerateHash(rawToken)));
	}

	private string GenerateRaw()
	{
		var randomBytes = new byte[32];

		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomBytes);

		return Convert.ToBase64String(randomBytes);

	}

	public string GenerateHash(string rawToken)
	{
		using var sha = SHA256.Create();

		var hashBytes = sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(rawToken));

		return Convert.ToBase64String(hashBytes);
	}
}
