namespace Liaro.Application.Account.Commands.UserLogout;

public sealed record UserLogoutCommandRequest(int UserId, string RefreshToken) : IRequest
{
    public static UserLogoutCommandRequest Create(int userId, string refreshToken)
        => new(userId, refreshToken);
}