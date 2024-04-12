namespace Liaro.Infrastructure.Persistance.Configuration;


internal sealed class ShortLinkConfig : IEntityTypeConfiguration<ShortLink>
{
    public void Configure(EntityTypeBuilder<ShortLink> builder)
    {
        // This line will filter soft deleted ShortLinks when fetching from db!
        builder.HasQueryFilter(sl => !sl.IsDeleted);
        builder.HasIndex(sl => sl.Source).IsUnique();
    }
}