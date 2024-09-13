namespace Liaro.Domain.Repository;

public interface IShortLinkRepository
{
    void Add(ShortLink shortLink);
    void Remove(ShortLink shortLink);
    void Update(ShortLink shortLink);
    Task<ShortLink> FindAsync(string source, CancellationToken token);
    Task<ShortLink> FindAsync(int id, CancellationToken token);
    Task<bool> AnyAsync(string source, CancellationToken token);
    Task<string> GetTarget(string source, CancellationToken token);
}