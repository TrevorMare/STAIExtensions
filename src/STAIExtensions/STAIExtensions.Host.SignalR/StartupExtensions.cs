using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace STAIExtensions.Host.SignalR;

public static class StartupExtensions
{

    public static IServiceCollection UseSTAISignalR(this IServiceCollection services)
    {

        services.AddHostedService<Services.HubContextNotificationService>();
        
        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "application/octet-stream" });
        });
        
        services.AddCors(options =>
        {
            options.AddPolicy(name: "SignalRCorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials();
                });
        });
        
        services.AddSignalR();
        
        return services;
    }

    public static IApplicationBuilder MapSTAISignalRHubs(this IApplicationBuilder app)
    {
        app.UseCors("SignalRCorsPolicy");
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<Hubs.STAIExtensionsHub>("/STAIExtensionsHub");
            
            endpoints.MapFallbackToFile("index.html");
        });
        
        return app;
    }
    
}