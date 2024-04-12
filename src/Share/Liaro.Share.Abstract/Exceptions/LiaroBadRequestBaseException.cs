namespace LiaroShare.Exceptions;


public abstract class LiaroBadRequestBaseException : Exception
{
    protected LiaroBadRequestBaseException(string message) : base(message) {}
}