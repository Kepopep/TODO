using Microsoft.AspNetCore.Mvc;
using TODO.API.Requests;
using TODO.Application.HabitLog;
using TODO.Application.HabitLog.Create;
using TODO.Application.HabitLog.Delete;
using TODO.Application.HabitLog.GetById;
using TODO.Application.HabitLog.GetPaged;
using TODO.Application.HabitLog.GetDay;
using TODO.Application.HabitLog.Update;

namespace TODO.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HabitLogsController : ControllerBase
{
    private readonly ICreateHabitLogService _createHabitLogService;
    private readonly IGetHabitLogByIdService _getHabitLogByIdService;
    private readonly IGetHabitLogPagedService _getHabitLogPagedService;
    private readonly IGetHabitLogByDayService _getHabitLogByDayService;
    private readonly IDeleteHabitLogService _deleteHabitLogService;
    private readonly IUpdateHabitLogService _updateHabitLogService;
    private readonly IUserContext _userContext;

    public HabitLogsController(
        ICreateHabitLogService createHabitLogService,
        IGetHabitLogByIdService getHabitLogByIdService,
        IGetHabitLogPagedService getHabitLogPagedService,
        IGetHabitLogByDayService getHabitLogByDayService,
        IDeleteHabitLogService deleteHabitLogService,
        IUpdateHabitLogService updateHabitLogService,
        IUserContext userContext)
    {
        _createHabitLogService = createHabitLogService;
        _getHabitLogByIdService = getHabitLogByIdService;
        _getHabitLogPagedService = getHabitLogPagedService;
        _getHabitLogByDayService = getHabitLogByDayService;
        _deleteHabitLogService = deleteHabitLogService;
        _updateHabitLogService = updateHabitLogService;
        _userContext = userContext;
    }

    /// <summary>
    /// Создает новую запись журнала привычки
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(HabitLogDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHabitLogRequest request)
    {
        var createDto = new CreateHabitLogDto(request.HabitId, request.Date);
        var habitLog = await _createHabitLogService.ExecuteAsync(_userContext.UserId, createDto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = habitLog.Id },
            habitLog);
    }

    /// <summary>
    /// Получает запись журнала привычки по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetHabitLogByIdDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var habitLog = await _getHabitLogByIdService.ExecuteAsync(id);

        if (habitLog.HabitLog == null)
        {
            return NotFound();
        }

        return Ok(habitLog);
    }

    /// <summary>
    /// Получает все записи журнала привычки для пользователя
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(HabitLogDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery]GetHabitLogsPagedRequest request)
    {
        var dto = new GetHabitLogPagedServiceDto(_userContext.UserId, request.Page, request.PageSize);
        var habitLog = await _getHabitLogPagedService.ExecuteAsync(dto);

        return Ok(habitLog);
    }

    /// <summary>
    /// Получает все записи журнала привычек для пользователя за конкретный день
    /// </summary>
    [HttpGet("day/{date}")]
    [ProducesResponseType(typeof(List<HabitLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDay(DateOnly date)
    {
        var dto = new GetHabitLogByDayServiceDto(_userContext.UserId, date);
        var habitLogs = await _getHabitLogByDayService.ExecuteAsync(dto);

        return Ok(habitLogs);
    }

    /// <summary>
    /// Обновляет запись журнала привычки по идентификатору
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHabitLogRequest request)
    {
        var dto = new UpdateHabitLogDto(_userContext.UserId, id, request.Date, request.HabitId);
        await _updateHabitLogService.ExecuteAsync(dto);

        return NoContent();
    }


    /// <summary>
    /// Удаляет запись журнала привычки по идентификатору
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var dto = new DeleteHabitLogServiceDto(_userContext.UserId, id);
        await _deleteHabitLogService.ExecuteAsync(dto);

        return NoContent();
    }
}