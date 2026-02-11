using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TODO.Application.Exceptions;

namespace TODO.Application.User.Context;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _accessor;

    public Guid UserId
    {
        get
        {
            var value = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? throw new UnauthorizedException("UserId claim missing");

            return Guid.Parse(value);
        }
    }

    private ClaimsPrincipal User =>
        _accessor.HttpContext?.User
        ?? throw new UnauthorizedException("No user context");

    public UserContext(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
}