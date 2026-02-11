using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TODO.Application.User.Login;

namespace TODO.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var accessToken = await _authService.Authenticate(new AuthServiceDto()
        {
            Email = request.Email,
            Password = request.Password
        });
        return Ok(accessToken);
    }
}