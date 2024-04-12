using LiaroShare.Exceptions;

namespace Liaro.Application.Account.Exceptions;


public class LoginTimeExpireException : LiaroBadRequestBaseException
{
    public LoginTimeExpireException()
        : base("Please try again in 5 minutes.") {}
}