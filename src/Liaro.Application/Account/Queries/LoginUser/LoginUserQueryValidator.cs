namespace Liaro.Application.Account.Queries.LoginUser;


public class LoginUserQueryValidator : AbstractValidator<LoginUserQueryRequest>
{
    public LoginUserQueryValidator()
    {
        RuleFor(u => u).NotNull().WithMessage("user is not set.");

        RuleFor(u => u.UserName).NotEmpty().NotNull()
            .WithMessage("user name ca'nt empty or null")
            .EmailAddress().WithMessage("Your email not valid.");

        
    }
}