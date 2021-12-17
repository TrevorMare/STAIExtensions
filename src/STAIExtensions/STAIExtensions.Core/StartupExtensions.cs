using Microsoft.Extensions.DependencyInjection;

namespace STAIExtensions.Core;

public static class StartupExtensions
{

    public static IServiceCollection UseSTAIExtensions(this IServiceCollection services)
    {

        services.AddScoped<Abstractions.ApiClient.IAIQueryApiClient, ApiClient.AIQueryApiClient>();
        services.AddScoped<Abstractions.Queries.IQueryBuilder, Queries.QueryBuilder>();
        services.AddScoped<Abstractions.Serialization.ITableRowDeserializer, Serialization.TableRowDeserializer>();
        
        return services;
    }
    
}