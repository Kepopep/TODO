using Microsoft.EntityFrameworkCore;
using TODO.Application.Exceptions;
using TODO.Application.User.Context;
using TODO.Infrastructure;

namespace TODO.Application.Habit.GetById;

public class GetHabitByIdService : IGetHabitByIdService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserContext _userContext;

    public GetHabitByIdService(AppDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<HabitDto> ExecuteAsync(GetHabitByIdDto dto)
    {
        // Шаг 1. Запрос строго по пользователю и Id
        var habit = await _dbContext.Habits
            .AsNoTracking()
            .FirstOrDefaultAsync(h =>
                h.UserId == _userContext.UserId &&
                h.Id == dto.HabitId);

        // Шаг 2. Проверка существования
        if (habit is null)
        {
            throw new NotFoundException("Habit not found");
        }

        // Шаг 3. Маппинг в DTO
        return new HabitDto(
            habit.Id,
            habit.UserId,
            habit.Name,
            habit.Frequency);
    }
}