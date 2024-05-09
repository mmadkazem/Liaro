using Liaro.Share.Abstract.Exceptions;

namespace Liaro.Application.ShortLinks.Exceptions;


public sealed class ShortLinkNotExistException : LiaroNotFoundBaseException
{
    public ShortLinkNotExistException()
        : base("This short link not exist for information!"){}
}