using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Host.SignalR.Identity;

namespace STAIExtensions.Host.SignalR;

public static class StartupExtensions
{

    public static IServiceCollection UseSTAISignalR(this IServiceCollection services, SignalRHostOptions? options = default)
    {
        
        options ??= new SignalRHostOptions();
        
        services.AddSingleton(options);
        services.AddSingleton<ISignalRUserGroups, SignalRUserGroups>();
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
        
        services.AddSingleton<IAuthorizationHandler, AuthTokenRequirementHandler>();
        
        services
            .AddAuthorization(options =>
            {
                options.AddPolicy("AuthTokenRequired", policy =>
                {
                    policy.Requirements.Add(new AuthTokenRequirement());
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