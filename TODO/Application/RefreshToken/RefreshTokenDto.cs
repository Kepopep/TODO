namespace TODO.Application.Refresh;

public record RefreshTokenDto(
	string RawToken,
	string TokenHash,
	DateTime CreateAt,
	DateTime ExpireAt);
