using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TODO.Application.Jwt.Factory;
using TODO.Application.User.Login;
using TODO.Domain.Entities;
using TODO.Infrastructure;

namespace TODO.Application.Refresh;

public class RefreshTokenService : IRefreshTokenService
{
	private readonly IRefreshTokenFactory _refreshFactory;

	private readonly AppDbContext _dbContext;

	private readonly UserManager<ApplicationUser> _userManager;

	public RefreshTokenService(
		IAccessTokenFactory accessFactory,
		IRefreshTokenFactory refreshCreateSystem,
		AppDbContext dbContext,
		UserManager<ApplicationUser> userManager)
	{
		_refreshFactory = refreshCreateSystem;
		_dbContext = dbContext;
		_userManager = userManager;
	}


	public async Task<string> CreateAsync(AuthServiceDto dto)
	{
		var userId = await GetUserIdFromEmail(dto.Email, dto.Password);

		var refreshToken = _refreshFactory.Create();

		await _dbContext.RefreshTokens.AddAsync(new RefreshToken(
			userId: userId,
			token: refreshToken.TokenHash,
			expireAt: refreshToken.ExpireAt));

		await _dbContext.SaveChangesAsync();

		return refreshToken.RawToken;
	}

	public async Task<string> RefreshAsync(string rawToken)
	{
		var userId = await GetUserIdFromToken(rawToken);

		var newRefreshToken = _refreshFactory.Create();

		await _dbContext.RefreshTokens.AddAsync(new RefreshToken(
			userId: userId,
			token: newRefreshToken.TokenHash,
			expireAt: newRefreshToken.ExpireAt));

		await _dbContext.SaveChangesAsync();

		return newRefreshToken.RawToken;
	}

	public async Task RevokeAsync(string rawToken)
	{
		var hashToken = _refreshFactory.Create(rawToken);

		var token = await _dbContext.RefreshTokens.
				AsNoTracking().
				FirstOrDefaultAsync(t => t.TokenHash == hashToken.TokenHash);

		if (token == null)
		{
			throw new AuthenticationException("No hash found");
		}

		token.ExpireAt = DateTime.UtcNow;

		_dbContext.RefreshTokens.
			Update(token);

		await _dbContext.SaveChangesAsync();
	}

	public async Task<string> ValidateAsync(string rawToken)
	{
		var virtualToken = _refreshFactory.Create(rawToken);

		if (!_refreshFactory.Validate(rawToken, virtualToken.TokenHash))
		{
			throw new AuthenticationException("refreshToken not valid");
		}
		var token = await _dbContext.RefreshTokens.
				AsNoTracking().
				FirstOrDefaultAsync(t => t.TokenHash == virtualToken.TokenHash);


		if (token == null || token.IsExpired || token.IsRevoked)
		{
			throw new AuthenticationException("refreshToken not valid");
		}

		return token.UserId.ToString();
	}

	private async Task<Guid> GetUserIdFromEmail(string email, string password)
	{
		var user = await _userManager.FindByEmailAsync(email);

		if (user == null || !await _userManager.CheckPasswordAsync(user, password))
		{
			throw new AuthenticationException("authentication error");
		}

		return user.Id;
	}

	private async Task<Guid> GetUserIdFromToken(string rawRefreshToken)
	{
		var tokenHash = _refreshFactory.Create(rawRefreshToken).TokenHash;

		var dbToken = await _dbContext.RefreshTokens.
			Where(t => t.TokenHash == tokenHash).
			ToListAsync();

		var userToken = dbToken.FirstOrDefault();

		if (userToken == null || userToken.IsExpired || userToken.IsRevoked)
		{
			throw new AuthenticationException("authentication error");
		}

		return userToken.UserId;
	}
}
