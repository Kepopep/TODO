using Microsoft.AspNetCore.Identity;
using TODO.Application.Exceptions;
using TODO.Domain.Entities;

namespace TODO.Application.User.Create;

public class CreateUserService : ICreateUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateUserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto> ExecuteAsync(CreateUserServiceDto serviceDto)
    {
        // 1. Создаём доменный объект
        var user = new ApplicationUser(
            serviceDto.Email,
            serviceDto.Name);

        // 2. Создаём пользователя через Identity
        var result = await _userManager.CreateAsync(
            user,
            serviceDto.Password);

        if (!result.Succeeded)
        {
            throw new DomainException("Create user error");
        }

        return new UserDto(
            Id: user.Id, 
            Name: user.Name);
    }   
}