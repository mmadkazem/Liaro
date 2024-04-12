namespace Liaro.Application.ShortLinks.Queries.RedirectOtherId;


public record RedirectOtherIdCommandRequest(string Source) : IRequest<string>
{
    public static implicit operator string(RedirectOtherIdCommandRequest request)
        => request.Source;
}