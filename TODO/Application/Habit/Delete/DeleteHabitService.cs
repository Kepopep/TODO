using Microsoft.EntityFrameworkCore;
using TODO.Application.Exceptions;
using TODO.Application.User.Context;
using TODO.Infrastructure;

namespace TODO.Application.Habit.Delete;

public class DeleteHabitService : IDeleteHabitService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserContext _userContext;

    public DeleteHabitService(AppDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task ExecuteAsync(DeleteHabitServiceDto dto)
    {
        // Шаг 1. Получение сущности
        var habit = await _dbContext.Habits
            .FirstOrDefaultAsync(h =>
                h.Id == dto.HabitId &&
                h.UserId == _userContext.UserId);

        // Шаг 2. Проверка существования и доступа
        if (habit is null)
        {
            throw new NotFoundException("Habit not found");
        }

        // Шаг 3. Удаление
        _dbContext.Habits.Remove(habit);

        // Шаг 4. Сохранение изменений
        await _dbContext.SaveChangesAsync();
    }
}