using STAIExtensions.Abstractions.Data;
using STAIExtensions.Core.DataSets.Options;
using STAIExtensions.Data.AzureDataExplorer;

namespace STAIExtensions.Host.Services;

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

        using (var scope = _serviceProvider.CreateScope())
        {
            
              
            var telemetryLoader = new STAIExtensions.Data.AzureDataExplorer.TelemetryLoader(new TelemetryLoaderOptions("2h1cqet6ae4nua4fjc0zqux14d2o1pb3uguavjts", "e666c38b-3ced-4bad-9661-ddc4dd01bb5b"));
            _ds = new Core.DataSets.DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

            _ds.StartAutoRefresh(TimeSpan.FromSeconds(30), cancellationToken);
        
            var z = new  Core.DataSets.DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");
        }
        
      

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _ds.StopAutoRefresh();
        return Task.CompletedTask;
    }
}