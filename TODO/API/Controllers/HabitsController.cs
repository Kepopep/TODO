using Microsoft.AspNetCore.Mvc;
using TODO.API.Requests;
using TODO.Application;
using TODO.Application.Habit;
using TODO.Application.Habit.Check;
using TODO.Application.Habit.Create;
using TODO.Application.Habit.Delete;
using TODO.Application.Habit.GetById;
using TODO.Application.Habit.GetPaged;
using TODO.Application.Habit.Update;

namespace TODO.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HabitsController : ControllerBase
{
    private readonly ICreateHabitService _createHabitService;
    private readonly IGetHabitByIdService _getHabitByIdService;
    private readonly IGetHabitPagedService _getHabitPagedService;
    private readonly IUpdateHabitService _updateHabitService;
    private readonly IDeleteHabitService _deleteHabitService;
    private readonly ICheckHabitService _checkHabitStatusService;

    public HabitsController(
        ICreateHabitService createHabitService,
        IGetHabitByIdService getHabitByIdService,
        IGetHabitPagedService getHabitPagedService,
        IUpdateHabitService updateHabitService,
        IDeleteHabitService deleteHabitService,
        ICheckHabitService checkHabitStatusService)
    {
        _createHabitService = createHabitService;
        _getHabitByIdService = getHabitByIdService;
        _getHabitPagedService = getHabitPagedService;
        _updateHabitService = updateHabitService;
        _deleteHabitService = deleteHabitService;
        _checkHabitStatusService = checkHabitStatusService;
    }

    /// <summary>
    /// Создает новую привычку
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(HabitDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateHabitRequest request)
    {
        // var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        // if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        // {
        //     return Unauthorized();
        // }
        
        //var createRequest = new CreateHabitServiceDto(userId, request.Name, request.Frequency);
        var createRequest = new CreateHabitServiceDto(Guid.Parse("d82b825f-40c3-4262-b367-55db930f0dc5"), request.Name, request.Frequency);
        var habit = await _createHabitService.ExecuteAsync(createRequest);

        return CreatedAtAction(
            nameof(GetById),
            new { id = habit.Id },
            habit);
    }

    /// <summary>
    /// Получает привычку по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HabitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }
        
        var habit = await _getHabitByIdService.ExecuteAsync(new GetHabitByIdDto(userId, id));

        //TODO add not found exeption

        return Ok(habit);
    }

    /// <summary>
    /// Получает список привычек с пагинацией
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<HabitDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetHabitsPagedRequest request)
    {
        var olegId = "d82b825f-40c3-4262-b367-55db930f0dc5";
        //var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        //{
            //return Unauthorized();
        //}
        
        var dto = new GetHabitPagedServiceDto(Guid.Parse(olegId), request.Page, request.PageSize);
        var habits = await _getHabitPagedService.ExecuteAsync(dto);

        return Ok(habits);
    }

    /// <summary>
    /// Обновляет информацию о привычке
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHabitRequest request)
    {
        // var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        // if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        // {
        //     return Unauthorized();
        // }
        
        var olegId = "d82b825f-40c3-4262-b367-55db930f0dc5";
        var updateDto = new UpdateHabitDto(Guid.Parse(olegId), id, request.Name, request.Frequency);
        
        await _updateHabitService.ExecuteAsync(updateDto);

        return NoContent();
    }

    /// <summary>
    /// Удаляет привычку по идентификатору
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }
        
        var dto = new DeleteHabitServiceDto(userId, id);

        await _deleteHabitService.ExecuteAsync(dto);

        return NoContent();
    }

    /// <summary>
    /// Проверяет или отменяет выполнение привычки на текущую дату
    /// </summary>
    [HttpPut("{habitId:guid}/check")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckStatus(Guid habitId, [FromBody] CheckHabitRequest request)
    {
        // var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        // if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        // {
        //     return Unauthorized();
        // }

        var olegId = "d82b825f-40c3-4262-b367-55db930f0dc5";
        var dto = new CheckHabitDto(Guid.Parse(olegId), habitId, request.IsChecked);
        await _checkHabitStatusService.ExecuteAsync(dto);

        return NoContent();
    }
}