using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TODO.Application;
using TODO.Application.Jwt.Factory;
using TODO.Domain.Entities;
using TODO.Infrastructure;

namespace TODO;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<ApiExceptionHandler>();
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddDbContext<AppIdentityDbContext>(options => 
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 3;
        }). AddEntityFrameworkStores<AppIdentityDbContext>().
            AddDefaultTokenProviders();
        
        builder.Services.AddApplicationServices();

        builder.Services.AddScoped<IUserContext, FakeUserContext>();
        builder.Services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();
        
        app.Run();
    }
}