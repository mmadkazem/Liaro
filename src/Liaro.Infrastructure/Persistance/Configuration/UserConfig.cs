namespace Liaro.Infrastructure.Persistance.Configuration;



internal sealed class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(e => e.Username)
            .HasMaxLength(450)
            .IsRequired();

        builder
            .HasIndex(e => e.Username)
            .IsUnique();

        builder
            .Property(e => e.Password)
            .IsRequired();

        builder
            .Property(e => e.SerialNumber)
            .HasMaxLength(450);
    }
}