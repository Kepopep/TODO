using TODO.Application.HabitLog;
using TODO.Application.HabitLog.Create;
using TODO.Application.HabitLog.Delete;
using TODO.Application.HabitLog.GetPaged;

namespace TODO.Application.Habit.Check;

public class CheckHabitService : ICheckHabitService
{
    private readonly ICreateHabitLogService _createHabitLogService;
    private readonly IGetHabitLogPagedService _getHabitLogPagedService;
    private readonly IDeleteHabitLogService _deleteHabitLogService;

    private readonly DateOnly _currentDate;

    public CheckHabitService(
        ICreateHabitLogService createHabitLogService,
        IGetHabitLogPagedService getHabitLogPagedService,
        IDeleteHabitLogService deleteHabitLogService)
    {
        _createHabitLogService = createHabitLogService;
        _getHabitLogPagedService = getHabitLogPagedService;
        _deleteHabitLogService = deleteHabitLogService;
        
        _currentDate = DateOnly.FromDateTime(DateTime.Now);
    }

    public async Task ExecuteAsync(CheckHabitDto dto)
    {
        var existingLogPage = await _getHabitLogPagedService.ExecuteAsync(new GetHabitLogPagedServiceDto(dto.UserId, 1, 1));

        if(existingLogPage.Items.Count != 0 == dto.IsChecked)
        {
            return;
        }

        if (dto.IsChecked)
        {
            await CreateLog(dto);
        }
        else
        {
            await DeleteLog(dto);
        }
    }

    private async Task DeleteLog(CheckHabitDto dto)
    {
        // Uncheck the habit by deleting the log entry for today
        var pagedResult = await _getHabitLogPagedService.ExecuteAsync(
            new GetHabitLogPagedServiceDto(dto.UserId, 1, 100));

        var todayLog = pagedResult.Items.FirstOrDefault(
            log => log.HabitId == dto.HabitId && log.Date == _currentDate);

        if (todayLog != null)
        {
            var deleteDto = new DeleteHabitLogServiceDto(dto.UserId, todayLog.Id);
            await _deleteHabitLogService.ExecuteAsync(deleteDto);
        }
    }

    private async Task CreateLog(CheckHabitDto dto)
    {
        var createDto = new CreateHabitLogDto(dto.HabitId, _currentDate);
        await _createHabitLogService.ExecuteAsync(dto.UserId, createDto);
    }
}