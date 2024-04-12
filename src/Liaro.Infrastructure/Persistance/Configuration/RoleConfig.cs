namespace Liaro.Infrastructure.Persistance.Configuration;


internal sealed class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .Property(e => e.Name)
            .HasMaxLength(450)
            .IsRequired();

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}