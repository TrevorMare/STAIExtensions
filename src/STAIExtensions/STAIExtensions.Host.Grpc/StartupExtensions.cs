using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace STAIExtensions.Host.Grpc;

public static class StartupExtensions
{
    
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