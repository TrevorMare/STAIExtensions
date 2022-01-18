using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace STAIExtensions.Host.Api;

/// <summary>
/// An extension class that assists in registering the Api Controllers and Security for the package
/// </summary>
public static class StartupExtensions
{
    
    /// <summary>
    /// Registers and attaches the required options for the Api Controllers
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsBuilder">The options builder</param>
    /// <returns></returns>
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