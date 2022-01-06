
using Examples.Grpc.Host.Console;
using Examples.Grpc.Host.Console.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using STAIExtensions.Core;
using STAIExtensions.Host.Grpc;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();
                
                // Add application insights to push sample telemetry
                TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
                configuration.InstrumentationKey = "c065e6f4-03cd-472e-94fd-c0518f8463f3";
                configuration.TelemetryInitializers.Add(new TelemetryInitializer());
                var telemetryClient = new TelemetryClient(configuration);
                services.AddScoped((s) => telemetryClient);
                
                var dsOptions = new STAIExtensions.Abstractions.Collections.DataSetCollectionOptions();
                var dsvOptions =
                    new STAIExtensions.Abstractions.Collections.ViewCollectionOptions(1000, false, true,
                        TimeSpan.FromMinutes(2));
                
                // Use the extensions project
                services.UseSTAIExtensions(() => dsOptions, () => dsvOptions);
                // Create the required services for the Grpc Channels and Authorization
                services.UseSTAIGrpc(new GrpcHostOptions(BearerToken: "ABC"));

                services.AddLogging(configure => configure.AddConsole());
                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                
                // Add a background service that will read and update datasets
                services.AddHostedService<ServiceRunner>();
            }).ConfigureWebHostDefaults((webBuilder) =>
            {
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    // Map the Grpc Controllers
                    app.MapSTAIGrpc();
                });
                // This will create an Grpc Endpoint by default on https://localhost:5001
                webBuilder.ConfigureKestrel(options =>
                    options.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2));
            });
}







