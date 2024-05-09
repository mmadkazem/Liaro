namespace Liaro.Infrastructure.ExternalServices;

public static class ExternalServicesExtension
{
    internal static IServiceCollection RegisterExternalServices(this IServiceCollection services)
    {
        // services.AddScoped<ITokenValidatorService, TokenValidatorService>();
        services.AddSingleton<IDbInitializerService, DbInitializerService>();
        services.AddScoped<ITokenFactoryService, TokenFactoryService>();
        services.AddScoped<IKavenegarService, KavenegarService>();
        services.AddSingleton<ISecurityService, SecurityService>();

        return services;
    }
}