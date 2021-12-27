using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace STAIExtensions.Host.Api;

public static class StartupExtensions
{
    
    public static IServiceCollection UseSTAIExtensionsApiHost(this IServiceCollection services,
        Func<ApiOptions>? optionsBuilder = default)
    {
        services.AddMvc()		
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddApplicationPart(typeof(Controllers.ViewController).Assembly);

        var options = optionsBuilder?.Invoke() ?? new ApiOptions();
        services.AddScoped<ApiOptions>((s) => options);
        
        return services;
    }
    
}