namespace Liaro.Infrastructure.Persistance.Repository;


public sealed class ShortLinkRepository(ApplicationDbContext context) : IShortLinkRepository
{
    private readonly ApplicationDbContext _context = context;

    public void Add(ShortLink shortLink)
        => _context.ShortLinks.Add(shortLink);

    public async Task<bool> AnyAsync(string source)
        => await _context.ShortLinks
                    .AsQueryable()
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Source == source);

    public async Task<ShortLink> FindAsync(string source)
        => await _context.ShortLinks
                    .AsQueryable()
                    .Where(s => s.Source == source)
                    .SingleOrDefaultAsync();

    public async Task<ShortLink> FindAsync(int id)
        => await _context.ShortLinks
                    .AsQueryable()
                    .Where(s => s.Id == id)
                    .SingleOrDefaultAsync();

    public void Remove(ShortLink shortLink)
        => _context.ShortLinks.Remove(shortLink);

    public void Update(ShortLink shortLink)
        => _context.ShortLinks.Update(shortLink);
}