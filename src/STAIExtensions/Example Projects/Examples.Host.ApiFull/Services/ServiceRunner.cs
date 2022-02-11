using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Data.AzureDataExplorer;
using STAIExtensions.Default.DataSets.Options;

namespace Examples.Host.ApiFull.Services;


public class ServiceRunner : IHostedService
{
    private IDataSet _ds;
    private readonly string _apiKey;
    private readonly string _appId;
    private readonly TelemetryClient? _telemetryClient;

    public ServiceRunner(IConfiguration configuration)
    {
        _apiKey = configuration["ReadTelemetryApiKey"];
        _appId = configuration["ReadTelemetryAppId"];
        _telemetryClient = STAIExtensions.Abstractions.DependencyExtensions.TelemetryClient;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // 1) Create a new telemetry loader instance
        var telemetryLoader = new STAIExtensions.Data.AzureDataExplorer.TelemetryLoader(
            new TelemetryLoaderOptions(_apiKey, _appId));
        
        // 2) Register a new dataset with the telemetry loader
        _ds = new STAIExtensions.Default.DataSets.DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(),
            "DataContractDataSet");
        
        // 3) Start the auto refresh on the dataset.
        _ds.StartAutoRefresh(TimeSpan.FromSeconds(30), cancellationToken);


        Task.Run(async () =>
        {
            int iteration = 0;
            while (cancellationToken.IsCancellationRequested == false)
            {
                bool isSuccess = (iteration < 10); 
                
                _telemetryClient?.TrackAvailability(nameof(ServiceRunner), DateTimeOffset.Now,
                    TimeSpan.FromMilliseconds(15), "localhost", isSuccess, 
                    isSuccess ? "Success" : "Failed");
                
                
                _telemetryClient?.TrackTrace($"This is a trace message tracked at {DateTime.Now}");
                _telemetryClient?.TrackTrace($"This is a information message tracked at {DateTime.Now}", SeverityLevel.Information);
                _telemetryClient?.TrackTrace($"This is a warning message tracked at {DateTime.Now}", SeverityLevel.Warning);
                _telemetryClient?.TrackTrace($"This is an error message tracked at {DateTime.Now}", SeverityLevel.Error);
                _telemetryClient?.TrackTrace($"This is an critical message tracked at {DateTime.Now}", SeverityLevel.Critical);
                
                await Task.Delay(30000, cancellationToken);
                
                iteration++;
                if (iteration >= 12) iteration = 0;
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _ds.StopAutoRefresh();
        return Task.CompletedTask;
    }
}