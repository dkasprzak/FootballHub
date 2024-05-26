using FootballHub.Application.Interfaces;
using FootballHub.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FootballHub.Application;

public static class DefaultDIConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICurrentAccountProvider, CurrentAccountProvider>();
        
        return services;
    }
}
