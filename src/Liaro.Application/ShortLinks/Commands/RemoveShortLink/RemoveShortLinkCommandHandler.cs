namespace Liaro.Application.ShortLinks.Commands.RemoveShortLink;


public sealed class RemoveShortLinkCommandHandler(IUnitOfWork uow)
    : IRequestHandler<RemoveShortLinkCommandRequest>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task Handle(RemoveShortLinkCommandRequest request, CancellationToken cancellationToken)
    {
        var shortLink = await _uow.ShortLinks.FindAsync(request);
        if (shortLink is null)
        {
            throw new ShortLinkNotExistException();
        }

        _uow.ShortLinks.Remove(shortLink);
        await _uow.SaveChangeAsync();
    }
}