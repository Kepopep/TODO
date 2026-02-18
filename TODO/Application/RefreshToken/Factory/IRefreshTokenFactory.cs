namespace TODO.Application.Refresh;

public interface IRefreshTokenFactory
{
	public RefreshTokenDto Create();

	public RefreshTokenDto Create(string rawToken);

	public bool Validate(string rawToken, string hashToken);
}
