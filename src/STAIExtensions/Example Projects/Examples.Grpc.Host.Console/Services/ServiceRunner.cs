using Microsoft.Extensions.Hosting;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Data.AzureDataExplorer;
using STAIExtensions.Default.DataSets.Options;

namespace Examples.Grpc.Host.Console.Services;

public class ServiceRunner : IHostedService
{
    private IDataSet _ds;
    private IServiceProvider _serviceProvider;

    public ServiceRunner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var telemetryLoader = new STAIExtensions.Data.AzureDataExplorer.TelemetryLoader(
            new TelemetryLoaderOptions("fn884tsrbltm7rwc7bggidd8nkbls6huvwlz05m1",
                "435b44c6-c1bd-4316-92a7-2760a0960cbe"));
        _ds = new STAIExtensions.Default.DataSets.DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");
        _ds.StartAutoRefresh(TimeSpan.FromSeconds(30), cancellationToken);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _ds.StopAutoRefresh();
        return Task.CompletedTask;
    }
}