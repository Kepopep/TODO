using Microsoft.EntityFrameworkCore;
using TODO.Application.Exceptions;
using TODO.Infrastructure;

namespace TODO.Application.HabitLog.Delete;

public class DeleteHabitLogService : IDeleteHabitLogService
{
    private readonly AppDbContext _dbContext;

    public DeleteHabitLogService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(DeleteHabitLogServiceDto dto)
    {
        // Шаг 1. Получение отметки выполнения
        var habitLog = await _dbContext.HabitLogs
            .FirstOrDefaultAsync(l =>
                l.Id == dto.HabitLogId &&
                l.UserId == dto.UserId);

        // Шаг 2. Проверка существования и доступа
        if (habitLog is null)
        {
            throw new DomainException("Habit completion not found");
        }

        // Шаг 3. Удаление отметки
        _dbContext.HabitLogs.Remove(habitLog);

        // Шаг 4. Сохранение изменений
        await _dbContext.SaveChangesAsync();
    }
}