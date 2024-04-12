namespace Liaro.Infrastructure.Persistance.Configuration;


internal sealed class UserRoleConfig : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RoleId });
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.RoleId);
        builder.Property(e => e.UserId);
        builder.Property(e => e.RoleId);

        builder.HasOne(d => d.Role)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(d => d.RoleId);

        builder.HasOne(d => d.User)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(d => d.UserId);
    }
}