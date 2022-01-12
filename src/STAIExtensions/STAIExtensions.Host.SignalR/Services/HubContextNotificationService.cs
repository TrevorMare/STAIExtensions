using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Host.SignalR.Hubs;
using STAIExtensions.Host.SignalR.Identity;

namespace STAIExtensions.Host.SignalR.Services;

internal class HubContextNotificationService : IHostedService
{

    #region Members

    private readonly IHubContext<STAIExtensionsHub, ISTAIExtensionsHubClient> _hubContext;
    private readonly IDataSetCollection _dataSetCollection;
    private readonly IViewCollection _viewCollection;
    private readonly ISignalRUserGroups _signalRUserGroups;

    #endregion
    
    #region ctor

    public HubContextNotificationService(IHubContext<STAIExtensionsHub, ISTAIExtensionsHubClient> hubContext,
                                         ISignalRUserGroups signalRUserGroups)
    {
        _dataSetCollection = Abstractions.DependencyExtensions.ServiceProvider.GetRequiredService<IDataSetCollection>();
        _viewCollection = Abstractions.DependencyExtensions.ServiceProvider.GetRequiredService<IViewCollection>();
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _signalRUserGroups = signalRUserGroups ?? throw new ArgumentNullException(nameof(signalRUserGroups));
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
        var groupNames = _signalRUserGroups.FindGroupNames(dataSetView.Id);

        if (_viewCollection.UseStrictViews)
        {
            // Check if there are group names
            var enumerable = groupNames as string[] ?? groupNames.ToArray();
        
            if (enumerable.Any())
            {
                foreach (var groupName in enumerable)
                {
                    await _hubContext.Clients.Group(groupName).OnDataSetViewUpdated(dataSetView.Id);    
                }
            }
            return;
        }
        
        // Fallback to notify everyone
        await _hubContext.Clients.All.OnDataSetViewUpdated(dataSetView.Id);
    }
    #endregion
    
   
}