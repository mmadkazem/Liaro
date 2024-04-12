namespace Liaro.Application.Account.Commands.UserLogout;

public sealed record UserLogoutCommandRequest(string RefreshToken) : IRequest
{
    public string? UserId { get; set; }
}