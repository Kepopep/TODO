using TODO.Application.User.Login;

namespace TODO.Application.Refresh;

public interface IRefreshTokenService
{
	public Task<string> CreateAsync(AuthServiceDto dto);

	public Task<string> RefreshAsync(string rawToken);

	public Task RevokeAsync(string rawToken);

	public Task<string> ValidateAsync(string rawToken);
}
