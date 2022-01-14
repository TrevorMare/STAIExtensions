
using Examples.Grpc.Host.Console;
using Examples.Grpc.Host.Console.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
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
                // Build a config object, using env vars and JSON providers.
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();
                
                // Add application insights to push sample telemetry
                services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();
                TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
                configuration.InstrumentationKey = config["InstrumentationKey"];
                configuration.TelemetryInitializers.Add(new TelemetryInitializer());
                var telemetryClient = new TelemetryClient(configuration);
                services.AddScoped((s) => telemetryClient);
                
                // Register the STAIExtensions Core Library with the options provided
                var dataSetCollectionOptions = new STAIExtensions.Abstractions.Collections.DataSetCollectionOptions(
                    null, 50);
                var viewCollectionOptions = new STAIExtensions.Abstractions.Collections.ViewCollectionOptions(
                    1000, 
                    false, 
                    true, 
                    TimeSpan.FromMinutes(15));

                services.UseSTAIExtensions(() => dataSetCollectionOptions, () => viewCollectionOptions);
                
                // Create the required services for the Grpc Channels and Authorization
                services.UseSTAIGrpc(new GrpcHostOptions(true, config["GrpcAuthorizationToken"]));

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







