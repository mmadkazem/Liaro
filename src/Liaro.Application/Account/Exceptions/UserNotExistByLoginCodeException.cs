namespace Liaro.Application.Account.Exceptions;


public sealed class InvalidLoginCodeException : LiaroBadRequestBaseException
{
    public InvalidLoginCodeException()
        : base("The entered code is wrong!") {}
}