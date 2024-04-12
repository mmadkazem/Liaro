namespace Liaro.Application.ShortLinks.Commands.CreateShortlink;


public record CreateShortlinkCommandRequest(string Source, string Target)
    : IRequest<string>
{
    public int UserId { get; set; }
}