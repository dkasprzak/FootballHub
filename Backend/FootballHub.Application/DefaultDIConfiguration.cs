using FluentValidation;
using FootballHub.Application.Helpers;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using FootballHub.Application.Services;
using FootballHub.Application.Validators;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FootballHub.Application;

public static class DefaultDIConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICurrentAccountProvider, CurrentAccountProvider>();
        
        return services;
    }
    
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(BaseQueryHandler));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}
