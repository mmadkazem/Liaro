
namespace Liaro.Application.ShortLinks.Queries.RedirectOtherId;


public sealed class RedirectOtherIdCommandHandler(IUnitOfWork uow)
        : IRequestHandler<RedirectOtherIdCommandRequest, string>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<string> Handle(RedirectOtherIdCommandRequest request, CancellationToken cancellationToken)
    {
        var shortLink = await _uow.ShortLinks.FindAsync(request);

        if (shortLink is null)
        {
            throw new ShortLinkNotExistException();
        }

        return shortLink.Target;
    }
}