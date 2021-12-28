using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Host.SignalR.Hubs;

namespace STAIExtensions.Host.SignalR.Services;

internal class HubContextNotificationService : IHostedService
{

    #region Members

    private readonly IHubContext<STAIExtensionsHub, ISTAIExtensionsHubClient> _hubContext;
    private readonly IDataSetCollection _dataSetCollection;
    private readonly IViewCollection _viewCollection;

    #endregion
    
    #region ctor

    public HubContextNotificationService(IHubContext<STAIExtensionsHub, ISTAIExtensionsHubClient> hubContext)
    {
        _dataSetCollection = Abstractions.DependencyExtensions.ServiceProvider.GetRequiredService<IDataSetCollection>();
        _viewCollection = Abstractions.DependencyExtensions.ServiceProvider.GetRequiredService<IViewCollection>();
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    #endregion

    #region Methods
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _viewCollection.OnDataSetViewUpdated += OnDataSetViewUpdated;
        _dataSetCollection.OnDataSetUpdated += OnDataSetUpdated;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _viewCollection.OnDataSetViewUpdated -= OnDataSetViewUpdated;
        _dataSetCollection.OnDataSetUpdated -= OnDataSetUpdated;
        return Task.CompletedTask;
    }
    
    private async void OnDataSetUpdated(IDataSet dataSet, EventArgs e)
    {
        await _hubContext.Clients.All.OnDataSetUpdated(dataSet.DataSetId);
    }

    private async void OnDataSetViewUpdated(IDataSetView dataSetView, EventArgs e)
    {
        await _hubContext.Clients.All.OnDataSetViewUpdated(dataSetView.Id);
    }
    #endregion
    
   
}