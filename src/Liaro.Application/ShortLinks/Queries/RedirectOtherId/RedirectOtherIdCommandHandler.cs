namespace Liaro.Application.ShortLinks.Queries.RedirectOtherId;


public sealed class RedirectOtherIdCommandHandler(IUnitOfWork uow)
        : IRequestHandler<RedirectOtherIdCommandRequest, string>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<string> Handle(RedirectOtherIdCommandRequest request, CancellationToken token)
        => await _uow.ShortLinks.GetTarget(request, token)
            ?? throw new ShortLinkNotExistException();
}