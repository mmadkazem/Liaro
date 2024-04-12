namespace Liaro.Infrastructure.Persistance.Configuration;


internal sealed class UserTokenConfig : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasOne(ut => ut.User)
            .WithMany(u => u.UserTokens)
            .HasForeignKey(ut => ut.UserId);

        builder.Property(ut => ut.RefreshTokenIdHash)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(ut => ut.RefreshTokenIdHashSource)
            .HasMaxLength(450);
    }
}