namespace Liaro.Infrastructure.ExternalServices;

public static class ExternalServicesExtension
{
    internal static IServiceCollection RegisterExternalServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenValidatorService, TokenValidatorService>();
        services.AddScoped<ITokenFactoryService, TokenFactoryService>();
        services.AddScoped<IKavenegarService, KavenegarService>();
        services.AddScoped<ISecurityService, SecurityService>();

        return services;
    }
}