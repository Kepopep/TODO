namespace TODO.Application.User.Login;

public interface IAuthService
{
    Task<string> Authenticate(AuthServiceDto dto);
}
