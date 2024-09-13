using Liaro.Application.Common;

namespace Liaro.Application.Account.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommandRequest>
{
    private readonly IUnitOfWork _uow;
    public RegisterUserCommandValidator(IUnitOfWork uow)
    {
        _uow = uow;
        RuleFor(u => u)
            .NotNull().WithMessage("Set the your information.");

        RuleFor(u => u.UserName)
            .MustAsync(UserNameAlreadyExist).WithMessage("This user name Already Exist.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Your Email cannot be empty.")
            .EmailAddress().WithMessage("Your email not valid.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Your password cannot be empty")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");

        RuleFor(u => u.Mobile)
            .NotEmpty().WithMessage("Your Mobile number cannot be empty.")
            .Must(StringUtils.IsValidPhone).WithMessage("Your Mobile number not valid.");
    }

    private async Task<bool> UserNameAlreadyExist(string userName, CancellationToken cancellationToken)
        => await _uow.Users.AnyAsyncUserNameAsync(userName);
}