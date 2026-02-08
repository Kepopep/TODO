using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TODO.Application.Exceptions;
using TODO.Domain.Entities;

namespace TODO.Application.User.Delete;

public class DeleteUserService : IDeleteUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task ExecuteAsync(DeleteUserServiceDto dto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == dto.UserId);

        if (user is null)
        {
            throw new DomainException("User not found");
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new DomainException("User deletion failed");
        }
    }
}