namespace Liaro.Application.Account.Commands.ChangePassword;


public record ChangePasswordCommandRequest(int UserId, string OldPassword, string NewPassword)
    : IRequest
{
    public static ChangePasswordCommandRequest Create(int userId, ChangePasswordDTO model)
        => new(userId, model.OldPassword, model.NewPassword);
}

public readonly record struct ChangePasswordDTO(string OldPassword, string NewPassword);