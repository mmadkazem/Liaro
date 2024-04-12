namespace Liaro.Application.Account.Exceptions;


public sealed class UserNotExistException : LiaroBadRequestBaseException
{
    public UserNotExistException()
        : base("Login again"){}
}