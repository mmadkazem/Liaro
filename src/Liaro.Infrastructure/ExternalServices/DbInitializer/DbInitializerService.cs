namespace Liaro.Infrastructure.ExternalServices.DbInitializer;

public class DbInitializerService : IDbInitializerService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ISecurityService _securityService;

    public DbInitializerService(
        IServiceScopeFactory scopeFactory,
        ISecurityService securityService)
    {
        _scopeFactory = scopeFactory;
        _scopeFactory.CheckArgumentIsNull(nameof(_scopeFactory));

        _securityService = securityService;
        _securityService.CheckArgumentIsNull(nameof(_securityService));
    }

    public void Initialize()
    {
        using var serviceScope = _scopeFactory.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.Migrate();
    }

    public void SeedData()
    {
        using var serviceScope = _scopeFactory.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        // Add default roles
        var adminRole = new Role { Name = CustomRoles.Admin };
        var userRole = new Role { Name = CustomRoles.User };
        var editorRole = new Role { Name = CustomRoles.Editor };

        if (!context.Roles.Any())
        {
            context.Add(adminRole);
            context.Add(userRole);
            context.Add(editorRole);
            context.SaveChanges();
        }

        // Add Admin user
        if (!context.Users.Any())
        {
            var adminUser = new User
            {
                Username = "Admin",
                Email = "Admin@Liaro.com",
                DisplayName = "ادمین",
                IsActive = true,
                LastLoggedIn = null,
                Password = _securityService.GetSha256Hash("1234"),
                SerialNumber = Guid.NewGuid().ToString("N")
            };
            context.Add(adminUser);

            var userRoles = new List<UserRole>()
                    {
                        new() { Role = adminRole, User = adminUser },
                        new() { Role = userRole, User = adminUser }
                    };

            userRoles.AddRange(userRoles);
            context.SaveChanges();
        }
    }
}
