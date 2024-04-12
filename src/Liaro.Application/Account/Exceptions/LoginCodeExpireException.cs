namespace Liaro.Application.Account.Exceptions;


public sealed class LoginCodeExpireException : LiaroBadRequestBaseException
{
    public LoginCodeExpireException()
        : base("The code sent has expired, get a new code again.") {}
}