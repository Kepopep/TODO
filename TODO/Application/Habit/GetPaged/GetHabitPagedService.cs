using Microsoft.EntityFrameworkCore;
using TODO.Infrastructure;

namespace TODO.Application.Habit.GetPaged;

public class GetHabitPagedService : IGetHabitPagedService
{
    private const int MaxPageSize = 50;
    private const int MinPageSize = 10;
    private readonly AppDbContext _dbContext;

    public GetHabitPagedService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<HabitDto>> ExecuteAsync(GetHabitPagedServiceDto dto)
    {
        // Шаг 1. Нормализация пагинации
        var page = dto.Page < 1 ? 1 : dto.Page;
        var pageSize = dto.PageSize <= 0 ? MinPageSize : dto.PageSize;

        if (pageSize > MaxPageSize)
            pageSize = MaxPageSize;

        // Шаг 2. Базовый IQueryable
        var query = _dbContext.Habits
            .AsNoTracking()
            .Where(h => h.UserId == dto.UserId)
            .OrderBy(h => h.Id);

        // Шаг 3. Запрос страницы + 1 элемент
        var habits = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize + 1)
            .ToListAsync();

        // Шаг 4. Проверка следующей страницы
        var hasNextPage = habits.Count > pageSize;
        if (hasNextPage)
            habits.RemoveAt(habits.Count - 1);

        var habitsId = habits
            .Select(h => h.Id)
            .ToList();

        var completedHabitsIds = await _dbContext.HabitLogs
            .AsNoTracking()
            .Where(l => l.UserId == dto.UserId && 
                habitsId.Contains(l.HabitId))
            .Select(h => h.HabitId)
            .Distinct()
            .ToListAsync();

        var completedSet = completedHabitsIds.ToHashSet();
        
        // Шаг 5. Маппинг
        var items = habits.Select(h =>
                new HabitDto(
                    h.Id,
                    h.UserId,
                    h.Name,
                    h.Frequency, 
                    completedSet.Contains(h.Id)))
            .ToList();

        // Шаг 6. Результат
        return new PagedResult<HabitDto>(
            items,
            page,
            pageSize,
            hasNextPage);
    }
}