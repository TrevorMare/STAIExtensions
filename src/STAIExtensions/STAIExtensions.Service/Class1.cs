using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Service;

public class Class1
{
    
    public static void Main()
    {
        // var serviceCollection = new ServiceCollection();
        // ConfigureServices(serviceCollection);
        //
        // serviceCollection.UseSTAIExtensions();
        //
        // var serviceProvider = serviceCollection.BuildServiceProvider();
        //
        // var telemetryLoader = new STAIExtensions.Data.AzureDataExplorer.TelemetryLoader(new TelemetryLoaderOptions(null, null));
        // var dataset =
        //     new Core.DataSets.DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");
        //
        // dataset.StartAutoRefresh(TimeSpan.FromSeconds(30));
        //
        // Console.ReadLine();
        //
        // dataset.StopAutoRefresh();
        
        
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
    }
    
}