using Microsoft.EntityFrameworkCore;
using TODO.Application.Exceptions;
using TODO.Application.User.Context;
using TODO.Domain.Entities;
using TODO.Infrastructure;

namespace TODO.Application.Habit.Create;

public class CreateHabitService : ICreateHabitService
{
    private readonly AppDbContext _dbContext;
    private readonly AppIdentityDbContext _identityDbContext;
    private readonly IUserContext _userContext;

    public CreateHabitService(AppDbContext dbContext, AppIdentityDbContext identityDbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _identityDbContext = identityDbContext;
        _userContext = userContext;
    }
    
    public async Task<HabitDto> ExecuteAsync(CreateHabitServiceDto dto)
    {
        // 1. Проверка пользователя (минимальная)
        var userExists = await _identityDbContext.Set<ApplicationUser>().
            AnyAsync(u => u.Id == _userContext.UserId);

        if (!userExists)
        {
            throw new NotFoundException("User not found");
        }

        // 2. Создание доменной сущности
        var habit = new Domain.Entities.Habit(
            _userContext.UserId,
            dto.Name,
            dto.Frequency);

        // 3. Сохранение
        _dbContext.Habits.Add(habit);
        await _dbContext.SaveChangesAsync();

        return new HabitDto(
            habit.Id, 
            habit.UserId, 
            habit.Name, 
            habit.Frequency);
    }
}