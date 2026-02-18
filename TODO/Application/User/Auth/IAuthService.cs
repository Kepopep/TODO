namespace TODO.Application.User.Login;

public interface IAccessTokenService
{
    Task<string> Authenticate(AuthServiceDto dto);

    Task<string> Authenticate(string rawRefreshToken);
}
