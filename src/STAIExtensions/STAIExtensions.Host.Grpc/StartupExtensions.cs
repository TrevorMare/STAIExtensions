using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace STAIExtensions.Host.Grpc;

public static class StartupExtensions
{
    
    public static IServiceCollection UseSTAIGrpc(this IServiceCollection services)
    {
        services.AddGrpc(
            options =>
            {
                options.Interceptors.Add<ExceptionInterceptor>();
            });
        services.AddGrpcReflection();

        return services;
    }

    public static IApplicationBuilder MapSTAIGrpc(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<Services.STAIExtensionsGrpcService>();
            if (env.IsDevelopment())
            {
                endpoints.MapGrpcReflectionService();
            }
        });
        
        return app;
    }
    
}