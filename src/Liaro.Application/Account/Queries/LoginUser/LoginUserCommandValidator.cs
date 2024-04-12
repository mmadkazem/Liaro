namespace Liaro.Application.Account.Queries.LoginUser;


public class LoginUserQueryValidator : AbstractValidator<LoginUserQueryRequest>
{
    public LoginUserQueryValidator()
    {
        RuleFor(u => u).NotNull().WithMessage("user is not set.");

        RuleFor(u => u.UserName)
        .NotEmpty().WithMessage("The username was entered incorrectly.")
        .MaximumLength(50).WithMessage("The length of the username must be less than 50");

        RuleFor(u => u.Password)
        .NotEmpty().WithMessage("The password was entered incorrectly.");
    }
}