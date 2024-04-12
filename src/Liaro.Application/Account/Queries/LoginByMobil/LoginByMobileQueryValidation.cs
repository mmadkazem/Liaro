namespace Liaro.Application.Account.Queries.LoginByMobil;


public sealed class LoginByMobileQueryValidation : AbstractValidator<LoginByMobileQueryRequest>
{
    public LoginByMobileQueryValidation()
    {
        RuleFor(u => u).NotNull().WithMessage("Information is not set.");

        RuleFor(u => u.Mobile)
            .NotEmpty().WithMessage("Your Mobile number cannot be empty.")
            .Must(StringUtils.IsValidPhone).WithMessage("Your Mobile number not valid.");

        RuleFor(u => u.Code)
        .NotEmpty().WithMessage("The Code was entered incorrectly.");
    }
}