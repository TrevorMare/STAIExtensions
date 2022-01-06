
using Examples.Grpc.Host.Console.Services;
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
                var dsOptions = new STAIExtensions.Abstractions.Collections.DataSetCollectionOptions();
                var dsvOptions = new STAIExtensions.Abstractions.Collections.ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(2));
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







