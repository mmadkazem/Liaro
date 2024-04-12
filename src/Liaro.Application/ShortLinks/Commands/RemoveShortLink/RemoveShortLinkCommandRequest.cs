namespace Liaro.Application.ShortLinks.Commands.RemoveShortLink;


public record RemoveShortLinkCommandRequest(string Source) : IRequest
{
    public static implicit operator string(RemoveShortLinkCommandRequest request)
        => request.Source;
}