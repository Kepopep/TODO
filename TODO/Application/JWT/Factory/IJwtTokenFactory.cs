
using Microsoft.AspNetCore.Identity;
using TODO.Domain.Entities;

namespace TODO.Application.Jwt.Factory;

public interface IJwtTokenFactory
{
    public string Create(ApplicationUser user);
}