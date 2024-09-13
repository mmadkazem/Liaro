namespace Liaro.Application.ShortLinks.Commands.CreateShortlink;


public sealed class CreateShortlinkCommandHandler(IUnitOfWork uow)
        : IRequestHandler<CreateShortlinkCommandRequest, string>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<string> Handle(CreateShortlinkCommandRequest request, CancellationToken token)
    {
        var source = request.Source;
        // if source of a shortlink was empty, it will generate 4chars unique key for it.
        if (source is null)
        {
            bool exist = true;
            string code = string.Empty;

            while (exist)
            {
                code = StringUtils.GetUniqueKey(4);
                exist = await _uow.ShortLinks.AnyAsync(code, token);
            }
            source = code;
        }
        _uow.ShortLinks.Add(new ShortLink
        {
            Source = source,
            Target = request.Target,
            CreatorUserId = request.UserId,
            Type = Domain.ShortLinks.ShortLinkType.Other
        });
        await _uow.SaveChangeAsync(token);
        return source;
    }
}