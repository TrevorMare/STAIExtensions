﻿using Microsoft.ApplicationInsights;
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
            while (cancellationToken.IsCancellationRequested == false)
            {
                _telemetryClient?.TrackAvailability(nameof(ServiceRunner), DateTimeOffset.Now,
                    TimeSpan.FromMilliseconds(1), "localhost", true);
                await Task.Delay(5000);
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