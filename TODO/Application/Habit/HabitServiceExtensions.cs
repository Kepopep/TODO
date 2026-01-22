using TODO.Application.Habit.Check;
using TODO.Application.Habit.Create;
using TODO.Application.Habit.Delete;
using TODO.Application.Habit.GetById;
using TODO.Application.Habit.GetPaged;
using TODO.Application.Habit.Update;

namespace TODO.Application.Habit;

public static class HabitServiceExtensions
{
    public static IServiceCollection AddHabitServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICreateHabitService, CreateHabitService>();
        
        services.AddScoped<IGetHabitByIdService, GetHabitByIdService>();
        services.AddScoped<IGetHabitPagedService, GetHabitPagedService>();
        
        services.AddScoped<IUpdateHabitService, UpdateHabitService>();
        services.AddScoped<ICheckHabitService, CheckHabitService>();
        
        services.AddScoped<IDeleteHabitService, DeleteHabitService>();

        return services;
    }
}