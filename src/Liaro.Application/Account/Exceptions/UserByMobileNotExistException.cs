namespace Liaro.Application.Account.Exceptions;


public sealed class UserByMobileNotExistException : LiaroBadRequestBaseException
{
    public UserByMobileNotExistException()
        : base("Please enter a valid mobile number."){}
}