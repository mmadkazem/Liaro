namespace Liaro.Application.Account.Commands.ChangePassword;


public record ChangePasswordCommandRequest(string OldPassword, string NewPassword)
    : IRequest
{
    public int UserId { get; set; }
}