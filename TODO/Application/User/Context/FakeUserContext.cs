namespace TODO.Application.User.Context;

public class FakeUserContext : IUserContext
{
    public Guid UserId => Guid.Parse("11111111-1111-1111-1111-111111111111");
}