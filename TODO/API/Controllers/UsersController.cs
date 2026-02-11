using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODO.API.Requests;
using TODO.Application.User;
using TODO.Application.User.Create;
using TODO.Application.User.Delete;

namespace TODO.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly ICreateUserService _createUserService;
    private readonly IDeleteUserService _deleteUserService;

    public UsersController(ICreateUserService createUserService, IDeleteUserService deleteUserService)
    {
        _createUserService = createUserService;
        _deleteUserService = deleteUserService;
    }

    /// <summary>
    ///  Создает пользователя с указанными параметрами
    /// </summary>
    [HttpPost("")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var dto = new CreateUserServiceDto(
            request.Email,
            request.UserName,
            request.Password);

        // Вся бизнес-логика создания пользователя
        // (валидация, Identity, сохранение) живёт в сервисе
        var user = await _createUserService.ExecuteAsync(dto);

        // Контроллер формирует корректный HTTP-ответ
        return CreatedAtAction(
            nameof(Create),
            new { id = user },
            new UserDto(Id: user.Id, Name: user.Name));
    }
    
    /// <summary>
    /// Удаляет пользователя с определенным идентификатором
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> Delete()
    {
        await _deleteUserService.ExecuteAsync();

        // Удаление ресурса → 204 No Content
        return NoContent();
    }
}