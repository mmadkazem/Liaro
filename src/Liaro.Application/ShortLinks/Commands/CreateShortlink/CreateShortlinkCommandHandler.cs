namespace Liaro.Application.ShortLinks.Commands.CreateShortlink;


public sealed class CreateShortlinkCommandHandler(IUnitOfWork uow)
        : IRequestHandler<CreateShortlinkCommandRequest, string>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<string> Handle(CreateShortlinkCommandRequest request, CancellationToken cancellationToken)
    {
        ShortLink shortLink = new()
        {
            Source = request.Source,
            Target = request.Target,
            Type = Domain.ShortLinks.ShortLinkType.Other,
            CreatorUserId = request.UserId
        };

        // if source of a shortlink was empty, it will generate 4chars unique key for it.
        if (string.IsNullOrEmpty(request.Source))
        {
            bool exist = true;
            string code = string.Empty;

            while (exist)
            {
                code = StringUtils.GetUniqueKey(4);
                exist = await _uow.ShortLinks.AnyAsync(request.Source);
            }
            shortLink.Source = code;
        }
        _uow.ShortLinks.Add(shortLink);
        await _uow.SaveChangeAsync();
        return shortLink.Source;
    }
}