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

    public async Task<bool> AnyAsyncUserName(string userName)
        => await _context.Users
                        .AsQueryable()
                        .Where(u => u.Username == userName).AnyAsync();

    public async Task<User> FindAsync(int userId)
        => await _context.Users
                        .AsQueryable()
                        .Where(x => x.Id == userId)
                        .FirstOrDefaultAsync();

    public async Task<User> FindAsyncByMobile(string mobile)
        => await _context.Users
                        .AsQueryable()
                        .Where(u => u.Mobile == mobile)
                        .FirstOrDefaultAsync();

    public async Task<User> FindByMobileAndLoginCode(string mobile, string code)
        => await _context.Users
                        .AsQueryable()
                        .Where(x => x.Mobile == mobile && x.LoginCode == code)
                        .FirstOrDefaultAsync();


    public async Task<User> FindUserAsync(string username, string password)
    {
        var passwordHash = _securityService.GetSha256Hash(password);
        return await _context.Users
                        .AsQueryable()
                        .Where(x => (x.Username.ToLower() == username.ToLower()
                                    || x.Email.ToLower() == username.ToLower()
                                    || x.Mobile == username)
                                && x.Password == passwordHash)
                        .FirstOrDefaultAsync();
    }

    public async Task<List<Role>> FindUserRolesAsync(int userId)
        => await _context.UserRoles
                        .AsQueryable()
                        .Include(x => x.Role)
                        .AsNoTracking()
                        .Where(x => x.UserId == userId)
                        .Select(x => x.Role)
                        .OrderBy(x => x.Name)
                        .ToListAsync();

    public void Remove(User user)
        => _context.Users.Remove(user);

    public void Update(User user)
        => _context.Users.Update(user);
}