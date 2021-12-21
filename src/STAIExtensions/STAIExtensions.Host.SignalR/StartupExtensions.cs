using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace STAIExtensions.Host.SignalR;

public static class StartupExtensions
{

    public static IServiceCollection UseSTAISignalR(this IServiceCollection services)
    {

        services.AddSignalR();
        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "application/octet-stream" });
        });
        
        return services;
    }

    public static IApplicationBuilder MapSTAISignalRHubs(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<Hubs.ViewHub>("/viewhub");
            
            //endpoints.MapBlazorHub();
            endpoints.MapFallbackToFile("index.html");
        });
        
        return app;
    }
    
}