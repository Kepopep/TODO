using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TODO.API.Response;
using TODO.Application.Refresh;
using TODO.Application.User.Login;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAccessTokenService _accessService;
    private readonly IRefreshTokenService _refreshService;

    public AuthenticationController(
            IAccessTokenService authService,
            IRefreshTokenService refreshService)
    {
        _accessService = authService;
        _refreshService = refreshService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var dto = new AuthServiceDto()
        {
            Email = request.Email,
            Password = request.Password
        };

        var accessToken = await _accessService.Authenticate(dto);
        var refreshToken = await _refreshService.CreateAsync(dto);

        var response = new AuthLoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return Ok(response);
    }

    [HttpPost()]
    public async Task<IActionResult> Refresh([FromBody] string refreshTokenRaw)
    {
        var accessToken = await _accessService.Authenticate(refreshTokenRaw);
        var newRefreshToken = await _refreshService.RefreshAsync(refreshTokenRaw);

        var response = new AuthLoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };

        return Ok(response);
    }
}
