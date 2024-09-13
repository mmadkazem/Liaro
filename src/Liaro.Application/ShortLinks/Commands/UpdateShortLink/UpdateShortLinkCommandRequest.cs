namespace Liaro.Application.ShortLinks.Commands.UpdateShortLink;

public record UpdateShortLinkCommandRequest(int Id, string Source, string Target)
    : IRequest
{
    public static UpdateShortLinkCommandRequest Create(int id, UpdateShortLinkDTO model)
        => new(id, model.Source, model.Target);
}

public readonly record struct UpdateShortLinkDTO(string Source, string Target);