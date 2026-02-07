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
    private readonly IUserContext _userContext;

    public HabitsController(
        ICreateHabitService createHabitService,
        IGetHabitByIdService getHabitByIdService,
        IGetHabitPagedService getHabitPagedService,
        IUpdateHabitService updateHabitService,
        IDeleteHabitService deleteHabitService,
        ICheckHabitService checkHabitStatusService,
        IUserContext userContext)
    {
        _createHabitService = createHabitService;
        _getHabitByIdService = getHabitByIdService;
        _getHabitPagedService = getHabitPagedService;
        _updateHabitService = updateHabitService;
        _deleteHabitService = deleteHabitService;
        _checkHabitStatusService = checkHabitStatusService;
        _userContext = userContext;
    }

    /// <summary>
    /// Создает новую привычку
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(HabitDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateHabitRequest request)
    {
        var createRequest = new CreateHabitServiceDto(_userContext.UserId, request.Name, request.Frequency);
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
        var habit = await _getHabitByIdService.ExecuteAsync(new GetHabitByIdDto(_userContext.UserId, id));

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
        var dto = new GetHabitPagedServiceDto(_userContext.UserId, request.Page, request.PageSize, request.Date);
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
        var updateDto = new UpdateHabitDto(_userContext.UserId, id, request.Name, request.Frequency);
        
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
        var dto = new DeleteHabitServiceDto(_userContext.UserId, id);

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
        var dto = new CheckHabitDto(_userContext.UserId, habitId, request.Date, request.IsChecked);
        await _checkHabitStatusService.ExecuteAsync(dto);

        return NoContent();
    }
}