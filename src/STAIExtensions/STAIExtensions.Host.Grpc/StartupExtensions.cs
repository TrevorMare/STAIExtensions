using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace STAIExtensions.Host.Grpc;

/// <summary>
/// An extension class that assists in registering the Grpc Host services and Security for the package
/// </summary>
public static class StartupExtensions
{
    /// <summary>
    /// Registers and attaches the required options for the Api Controllers
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection UseSTAIGrpc(this IServiceCollection services, GrpcHostOptions? options = default)
    {
        options ??= new GrpcHostOptions();

        services.AddScoped<GrpcHostOptions>((s) => options);
        
        services.AddGrpc(
            options =>
            {
                options.Interceptors.Add<AuthorizationInterceptor>();
                options.Interceptors.Add<ExceptionInterceptor>();
            });
        
        services.AddGrpcReflection();

        return services;
    }

    /// <summary>
    /// Maps the Grpc Endpoint services
    /// </summary>
    /// <param name="app"></param>
    /// <param name="mapGrpcReflectionService">A value indicating if the MapGrpcReflectionService will be included</param>
    /// <returns></returns>
    public static IApplicationBuilder MapSTAIGrpc(this IApplicationBuilder app, bool mapGrpcReflectionService = true)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<Services.STAIExtensionsGrpcService>();
            if (mapGrpcReflectionService == true)
            {
                endpoints.MapGrpcReflectionService();
            }
        });
        
        return app;
    }
    
}