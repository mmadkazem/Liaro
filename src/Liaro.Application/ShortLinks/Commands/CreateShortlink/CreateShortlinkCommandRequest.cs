namespace Liaro.Application.ShortLinks.Commands.CreateShortlink;


public record CreateShortlinkCommandRequest(int UserId, string? Source, string Target)
    : IRequest<string>
{
    public static CreateShortlinkCommandRequest Create(int userId, CreateShortlinkDTO model)
        => new(userId, model.Source, model.Target);
}

public readonly record struct CreateShortlinkDTO(string? Source, string Target);