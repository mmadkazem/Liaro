using System.Reflection;

namespace Liaro.Infrastructure.Persistance.Context;

public class ApplicationDbContext : DbContext
{
    //DbSets
    public virtual DbSet<User> Users { set; get; }
    public virtual DbSet<Role> Roles { set; get; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<UserToken> UserTokens { get; set; }
    public virtual DbSet<ShortLink> ShortLinks { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }


    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSaving()
    {
        CustomChangeTracker<ShortLink>();

    }


    private void CustomChangeTracker<T>() where T : class
    {
        foreach (var entry in ChangeTracker.Entries<T>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.CurrentValues["IsDeleted"] = false;
                    entry.CurrentValues["CreatedOn"] = DateTime.UtcNow;
                    entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                    entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;
                    break;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Custom application mappings

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
