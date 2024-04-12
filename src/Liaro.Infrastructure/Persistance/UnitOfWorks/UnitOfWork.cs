namespace Liaro.Infrastructure.Persistance.UnitOfWorks;


public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _users;
    private readonly IUserTokenRepository _userTokens;
    private readonly IShortLinkRepository _shortLinks;

    public UnitOfWork(ApplicationDbContext context,
            IUserRepository users,
            IUserTokenRepository userTokens,
            IShortLinkRepository shortLinks)
    {
        _context = context;
        _users = users;
        _userTokens = userTokens;
        _shortLinks = shortLinks;
    }

    public IUserRepository Users
        => _users;

    public IUserTokenRepository UserTokens
        => _userTokens;

    public IShortLinkRepository ShortLinks
        => _shortLinks;

    public async Task SaveChangeAsync()
        => await _context.SaveChangesAsync();
}