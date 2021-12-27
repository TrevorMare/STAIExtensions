using Microsoft.Extensions.DependencyInjection;

namespace STAIExtensions.Core;

public static class StartupExtensions
{

    public static IServiceCollection UseSTAIExtensions(this IServiceCollection services)
    {
        services.AddSingleton<Abstractions.Collections.IDataSetCollection, Collections.DataSetCollection>();
        services.AddSingleton<Abstractions.Collections.IViewCollection, Collections.ViewCollection>();
        
        Abstractions.DependencyExtensions.UseSTAIExtensionsAbstractions(services);
        
        return services;
    }
    
}