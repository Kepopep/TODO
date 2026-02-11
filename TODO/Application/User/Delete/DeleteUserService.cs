using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TODO.Application.Exceptions;
using TODO.Application.User.Context;
using TODO.Domain.Entities;

namespace TODO.Application.User.Delete;

public class DeleteUserService : IDeleteUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserContext _context;

    public DeleteUserService(UserManager<ApplicationUser> userManager, IUserContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task ExecuteAsync()
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == _context.UserId);

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