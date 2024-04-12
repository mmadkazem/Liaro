namespace Liaro.Application.Account.Queries.LoginByRefreshToken;


public sealed class LoginByRefreshTokenQueryValidator : AbstractValidator<LoginByRefreshTokenQueryRequest>
{
    public LoginByRefreshTokenQueryValidator()
    {
        RuleFor(u => u.RefreshToken)
            .NotEmpty().WithMessage("refreshToken is not set.");

        RuleFor(u => u.RefreshToken)
            .NotNull().WithMessage("refreshToken is not set.");
    }
}