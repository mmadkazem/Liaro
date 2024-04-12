using Liaro.Infrastructure.ExternalServices;
using Liaro.Infrastructure.Persistance;

namespace Liaro.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection RegisterInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterExternalServices()
                .RegisterPersistanceServices(configuration);

        return services;
    }
}