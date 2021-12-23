using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace STAIExtensions.Host.Api;

public static class StartupExtensions
{
    
    public static IServiceCollection UseSTAIExtensionsApiHost(this IServiceCollection services)
    {
        services.AddMvc()		
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddApplicationPart(typeof(Controllers.ViewController).Assembly);
        return services;
    }
    
}