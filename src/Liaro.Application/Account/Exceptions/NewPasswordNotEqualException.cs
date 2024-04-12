namespace Liaro.Application.Account.Exceptions;


public sealed class NewPasswordNotEqualException : LiaroBadRequestBaseException
{
    public NewPasswordNotEqualException()
        : base("This password not equals in confirm password."){}
}