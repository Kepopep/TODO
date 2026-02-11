using Microsoft.AspNetCore.Identity;
using TODO.Application.Exceptions;
using TODO.Application.Jwt.Factory;
using TODO.Domain.Entities;

namespace TODO.Application.User.Login;

public class AuthService : IAuthService
{
    private readonly IUserContext _context;
    private readonly UserManager<ApplicationUser> _manager;
    private readonly IJwtTokenFactory _factory;

    public AuthService(IUserContext context, UserManager<ApplicationUser> manager, IJwtTokenFactory factory)
    {
        _context = context;
        _manager = manager;
        _factory = factory;
    }

    public async Task<string> Authenticate(AuthServiceDto dto)
    {
        var user = await _manager.FindByEmailAsync(dto.Email);

        if (user is null ||
            !await _manager.CheckPasswordAsync(user, dto.Password))
        {
            // TODO: Return a more specific exception for unauthorized access
            throw new UnauthorizedException("Invalid credentials");
        }

        return _factory.Create(user);
    }
}