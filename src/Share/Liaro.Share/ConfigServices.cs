using Liaro.Share.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Liaro.Share;

public static class ConfigServices
{
    public static IServiceCollection RegisterSharedServices(this IServiceCollection services)
    {
        services.AddScoped<ValidationsExceptionMiddleware>();
        services.AddScoped<BadRequestExceptionMiddleware>();
        services.AddScoped<NotFoundExceptionMiddleware>();
        return services;
    }
    public static IApplicationBuilder UseShared(this IApplicationBuilder app)
    {
        app.UseMiddleware<ValidationsExceptionMiddleware>();
        app.UseMiddleware<BadRequestExceptionMiddleware>();
        app.UseMiddleware<NotFoundExceptionMiddleware>();
        return app;
    }
}