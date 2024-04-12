namespace Liaro.Application.Account.Commands.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommandRequest>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(u => u.OldPassword)
            .NotEmpty().WithMessage("Your old password cannot be empty");

        RuleFor(u => u.NewPassword)
            .NotEmpty().WithMessage("Your new password cannot be empty")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
    }
}