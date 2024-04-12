namespace Liaro.Application.ShortLinks.Commands.UpdateShortLink;

public record UpdateShortLinkCommandRequest(int Id, string Source, string Target)
    : IRequest
{
    public int UserId { get; set; }
}