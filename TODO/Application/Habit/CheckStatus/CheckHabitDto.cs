using TODO.Application.HabitLog;

namespace TODO.Application.Habit.Check;

public record CheckHabitDto(
    Guid UserId,
    Guid HabitId,
    bool IsChecked);