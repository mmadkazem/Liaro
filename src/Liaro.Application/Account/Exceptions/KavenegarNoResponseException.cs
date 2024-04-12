using LiaroShare.Exceptions;

namespace Liaro.Application.Account.Exceptions;


public class KavenegarNoResponseException : LiaroBadRequestBaseException
{
    public KavenegarNoResponseException()
        : base("Something went wrong while sending the SMS!") {}
}