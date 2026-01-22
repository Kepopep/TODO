using TODO.Domain.Enum;

namespace TODO.Application.Habit;

public record HabitDto(
    Guid Id, 
    Guid UserId, 
    string Name,
    Frequency Frequency,
    bool IsChecked = false);