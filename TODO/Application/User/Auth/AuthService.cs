using Microsoft.AspNetCore.Identity;
using TODO.Application.Exceptions;
using TODO.Application.Jwt.Factory;
using TODO.Application.Refresh;
using TODO.Application.User.Context;
using TODO.Domain.Entities;

namespace TODO.Application.User.Login;

public class AuthService : IAccessTokenService
{
    private readonly UserManager<ApplicationUser> _manager;
    private readonly IAccessTokenFactory _accesFactory;

    private readonly IRefreshTokenService _refreshService;

    public AuthService(UserManager<ApplicationUser> manager, IAccessTokenFactory factory, IRefreshTokenService refreshService)
    {
        _manager = manager;
        _accesFactory = factory;
        _refreshService = refreshService;
    }

    public async Task<string> Authenticate(AuthServiceDto dto)
    {
        var user = await _manager.FindByEmailAsync(dto.Email);

        if (user is null ||
            !await _manager.CheckPasswordAsync(user, dto.Password))
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        return _accesFactory.Create(user.Id.ToString(), dto.Email);
    }

    public async Task<string> Authenticate(string rawRefreshToken)
    {
        var userId = await _refreshService.ValidateAsync(rawRefreshToken);

        var user = await _manager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        return _accesFactory.Create(userId, user.Email!);
    }
}
