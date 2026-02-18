
using Microsoft.AspNetCore.Identity;
using TODO.Domain.Entities;

namespace TODO.Application.Jwt.Factory;

public interface IAccessTokenFactory
{
    public string Create(string id, string email);
}
