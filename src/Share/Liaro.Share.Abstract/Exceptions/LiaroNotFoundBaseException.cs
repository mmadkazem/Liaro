namespace Liaro.Share.Abstract.Exceptions;


public abstract class LiaroNotFoundBaseException : Exception
{
    protected LiaroNotFoundBaseException(string message) : base(message) {}
}