namespace Liaro.Infrastructure.Persistance;


internal static class ConfigureServices
{
    public static IServiceCollection RegisterPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DI Repositories and UnitOfWork
        services.AddTransient<IUserTokenRepository, UserTokenRepository>();
        services.AddTransient<IShortLinkRepository, ShortLinkRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        // DI DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("LiaroDb")));
        return services;
    }
}