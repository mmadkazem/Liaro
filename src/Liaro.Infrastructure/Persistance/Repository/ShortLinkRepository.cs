namespace Liaro.Infrastructure.Persistance.Repository;


public sealed class ShortLinkRepository(ApplicationDbContext context) : IShortLinkRepository
{
    private readonly ApplicationDbContext _context = context;

    public void Add(ShortLink shortLink)
        => _context.ShortLinks.Add(shortLink);

    public async Task<bool> AnyAsync(string source, CancellationToken token)
        => await _context.ShortLinks.AsQueryable()
                                    .IgnoreQueryFilters()
                                    .AnyAsync(x => x.Source == source, token);

    public async Task<ShortLink> FindAsync(string source, CancellationToken token)
        => await _context.ShortLinks.AsQueryable()
                                    .Where(s => s.Source == source)
                                    .SingleOrDefaultAsync(token);

    public async Task<ShortLink> FindAsync(int id, CancellationToken token)
        => await _context.ShortLinks.AsQueryable()
                                    .Where(s => s.Id == id)
                                    .SingleOrDefaultAsync(token);

    public async Task<string> GetTarget(string source, CancellationToken token)
        => await _context.ShortLinks.AsQueryable()
                                    .Where(s => s.Source == source)
                                    .Select(s => s.Target)
                                    .FirstOrDefaultAsync(token);

    public void Remove(ShortLink shortLink)
        => _context.ShortLinks.Remove(shortLink);

    public void Update(ShortLink shortLink)
        => _context.ShortLinks.Update(shortLink);
}