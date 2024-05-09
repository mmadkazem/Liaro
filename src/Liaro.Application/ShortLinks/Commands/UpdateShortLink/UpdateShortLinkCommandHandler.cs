namespace Liaro.Application.ShortLinks.Commands.UpdateShortLink;


public sealed class UpdateShortLinkCommandHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateShortLinkCommandRequest>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task Handle(UpdateShortLinkCommandRequest request, CancellationToken cancellationToken)
    {

        var shortLink = await _uow.ShortLinks.FindAsync(request.Id);
        if (shortLink is null)
        {
            throw new ShortLinkNotExistException();
        }

        shortLink.Source = request.Source;
        shortLink.Target = request.Target;

        _uow.ShortLinks.Update(shortLink);
        await _uow.SaveChangeAsync();
    }
}