using Microsoft.EntityFrameworkCore;
using TODO.Application.Exceptions;
using TODO.Infrastructure;

namespace TODO.Application.HabitLog.Update;

public class UpdateHabitLogService : IUpdateHabitLogService
{
    private readonly AppDbContext _dbContext;

    public UpdateHabitLogService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(UpdateHabitLogDto dto)
    {
        // Step 1. Get the habit log
        var habitLog = await _dbContext.HabitLogs
            .FirstOrDefaultAsync(l =>
                l.Id == dto.HabitLogId &&
                l.UserId == dto.UserId);

        // Step 2. Check existence and access
        if (habitLog is null)
        {
            throw new DomainException("Habit log not found");
        }

        // Step 3. Verify that the habit exists and belongs to the user
        var habitExists = await _dbContext.Habits.AnyAsync(h =>
            h.Id == dto.HabitId &&
            h.UserId == dto.UserId);

        if (!habitExists)
        {
            throw new NotFoundException("Habit not found");
        }

        // Step 4. Check if a log for the same habit and date already exists for a different log entry
        var alreadyLogged = await _dbContext.HabitLogs.AnyAsync(l =>
            l.HabitId == dto.HabitId &&
            l.UserId == dto.UserId &&
            l.Date == dto.Date &&
            l.Id != dto.HabitLogId); // Exclude the current log being updated

        if (alreadyLogged)
            throw new DomainException("Habit already logged for this date");

        // Step 5. Update the habit log
        habitLog.Date = dto.Date;
        habitLog.HabitId = dto.HabitId;

        // Step 6. Save changes
        await _dbContext.SaveChangesAsync();
    }
}