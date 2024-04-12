namespace Liaro.Domain.Repository;

public interface IShortLinkRepository
{
    void Add(ShortLink shortLink);
    void Remove(ShortLink shortLink);
    void Update(ShortLink shortLink);
    Task<ShortLink> FindAsync(string source);
    Task<ShortLink> FindAsync(int id);
    Task<bool> AnyAsync(string source);
}