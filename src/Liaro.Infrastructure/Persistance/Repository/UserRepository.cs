namespace Liaro.Infrastructure.Persistance.Repository;


public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ISecurityService _securityService;

    public UserRepository(ApplicationDbContext context,
            ISecurityService securityService)
    {
        _context = context;

        _securityService = securityService;
        _securityService.CheckArgumentIsNull(nameof(_securityService));
    }

    public void Add(User user)
        => _context.Users.Add(user);

    public async Task<bool> AnyAsyncUserNameAsync(string userName, CancellationToken token)
        => await _context.Users
                        .AsQueryable()
                        .AnyAsync(u => u.Username == userName, token);

    public async Task<User> FindAsync(int userId, CancellationToken token)
        => await _context.Users
                        .AsQueryable()
                        .Where(x => x.Id == userId)
                        .FirstOrDefaultAsync(token);

    public async Task<User> FindAsyncByMobileAsync(string mobile, CancellationToken token)
        => await _context.Users
                        .AsQueryable()
                        .Where(u => u.Mobile == mobile)
                        .FirstOrDefaultAsync(token);

    public async Task<User> FindByMobileAndLoginCodeAsync(string mobile, string code, CancellationToken token)
        => await _context.Users
                        .AsQueryable()
                        .Where(x => x.Mobile == mobile && x.LoginCode == code)
                        .FirstOrDefaultAsync(token);


    public async Task<User> FindUserAsync(string username, string password, CancellationToken token)
    {
        var passwordHash = _securityService.GetSha256Hash(password);
        return await _context.Users
                        .AsQueryable()
                        .Where(x => (x.Username.ToLower() == username.ToLower()
                                    || x.Email.ToLower() == username.ToLower()
                                    || x.Mobile == username)
                                && x.Password == passwordHash)
                        .FirstOrDefaultAsync(token);
    }

    public async Task<List<Role>> FindUserRolesAsync(int userId, CancellationToken token)
        => await _context.UserRoles
                        .AsQueryable()
                        .Include(x => x.Role)
                        .AsNoTracking()
                        .Where(x => x.UserId == userId)
                        .Select(x => x.Role)
                        .OrderBy(x => x.Name)
                        .ToListAsync(token);

    public void Remove(User user)
        => _context.Users.Remove(user);

    public void Update(User user)
        => _context.Users.Update(user);
}