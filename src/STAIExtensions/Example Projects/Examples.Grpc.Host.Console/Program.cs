
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
                services.UseSTAIExtensions(() => dsOptions, () => dsvOptions);
                services.UseSTAIGrpc();
                services.AddLogging(configure => configure.AddConsole());
                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                services.AddHostedService<ServiceRunner>();
            }).ConfigureWebHostDefaults((webBuilder) =>
            {
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.MapSTAIGrpc();
                });
                webBuilder.ConfigureKestrel(options =>
                    options.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2));
            });
}







