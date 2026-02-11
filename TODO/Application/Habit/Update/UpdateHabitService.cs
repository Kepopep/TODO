using Microsoft.EntityFrameworkCore;
using TODO.Application.Exceptions;
using TODO.Application.HabitLog.Create;
using TODO.Application.User.Context;
using TODO.Domain;
using TODO.Infrastructure;

namespace TODO.Application.Habit.Update;

public class UpdateHabitService : IUpdateHabitService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserContext _userContext;

    public UpdateHabitService(AppDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task ExecuteAsync(UpdateHabitDto dto)
    {
        // Шаг 1. Получение сущности
        var habit = await _dbContext.Habits
            .FirstOrDefaultAsync(h =>
                h.Id == dto.HabitId &&
                h.UserId == _userContext.UserId);

        // Шаг 2. Проверка доступа и существования
        if (habit is null)
        {
            throw new NotFoundException("Habit not found");
        }

        // Шаг 3. Обновление состояния доменной сущности
        habit.Name = dto.Name;
        habit.Frequency = dto.Frequency;

        // Шаг 4. Сохранение изменений
        await _dbContext.SaveChangesAsync();
    }
}