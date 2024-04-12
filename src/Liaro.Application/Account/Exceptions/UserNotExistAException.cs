using LiaroShare.Exceptions;

namespace Liaro.Application.Account.Exceptions;


public class UserNotExistByUserNameAndPasswordException : LiaroBadRequestBaseException
{
    public UserNotExistByUserNameAndPasswordException() : base("Wrong username or password entered.") {}
}