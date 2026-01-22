namespace TODO.Application.Habit.Check;

public interface ICheckHabitService
{
    Task ExecuteAsync(CheckHabitDto dto);
}