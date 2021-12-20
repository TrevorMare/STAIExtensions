using Microsoft.Extensions.DependencyInjection;

namespace STAIExtensions.Core;

public static class StartupExtensions
{

    public static IServiceCollection UseSTAIExtensions(this IServiceCollection services)
    {
        Abstractions.DependencyExtensions.UseSTAIExtensions(services);        
        return services;
    }
    
}