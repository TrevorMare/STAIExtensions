using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.CQRS.DataSets.Queries;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;
using STAIExtensions.Host.SignalR.Identity;

namespace STAIExtensions.Host.SignalR.Hubs;


/// <summary>
/// The STAIExtensions SignalR hub. Because SignalR is implemented on a send message based protocol
/// and does not allow for a return value on methods, each function exposed here has a response function
/// to notify the attached client of the result.
/// </summary>
[Authorize(policy : "AuthTokenRequired")]
public class STAIExtensionsHub : Hub<ISTAIExtensionsHubClient>
{

    #region Members
    private readonly IMediator? _mediator;
    private readonly ISignalRUserGroups _signalRUserGroups;
    private readonly ILogger<STAIExtensionsHub>? _logger;
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public STAIExtensionsHub(ISignalRUserGroups signalRUserGroups)
    {
        _mediator = Abstractions.DependencyExtensions.Mediator;
        _signalRUserGroups = signalRUserGroups ?? throw new ArgumentNullException(nameof(signalRUserGroups));
        
        _logger = Abstractions.DependencyExtensions.CreateLogger<STAIExtensionsHub>();
        _telemetryClient = Abstractions.DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Creates a new view for the Owner
    /// </summary>
    /// <param name="viewType">The fully qualified type name</param>
    /// <param name="ownerId">The owner that the view belongs to</param>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task CreateView(string viewType, string ownerId, string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(CreateView)}");
        try
        {
            
            var response = await _mediator?.Send(new CreateViewCommand(viewType, ownerId))!;

            string groupName = $"{ownerId}_{response.Id}";

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            _signalRUserGroups.RegisterUserGroupView(response, groupName);

            await Clients.Caller.OnDataSetViewCreated(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(CreateView), new Dictionary<string, string>()
            {
                {"ViewId", viewType},
                {"OwnerId", ownerId},
                {"Success", (response != null).ToString()}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
        
    }

    /// <summary>
    /// Gets a view in it's current state from the collections
    /// </summary>
    /// <param name="viewId">The view Id to retrieve</param>
    /// <param name="ownerId">The owner of the view</param>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task GetView(string viewId, string ownerId, string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(GetView)}");

        try
        {
            var response = await _mediator?.Send(new GetDataViewQuery(viewId, ownerId))!;
            await Clients.Caller.OnGetViewResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(GetView), new Dictionary<string, string>()
            {
                {"ViewId", viewId},
                {"OwnerId", ownerId}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
    }
    
    /// <summary>
    /// Lists all the registered data sets in the system
    /// </summary>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task ListDataSets(string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(ListDataSets)}");
        try
        {
            
            var response = await _mediator?.Send(new ListDataSetsQuery())!;
            await Clients.Caller.OnListDataSetsResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(ListDataSets), new Dictionary<string, string>()
            {
                {"Response", ConvertToJsonObject(response)}
            });
            
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
    }
    
    /// <summary>
    /// Gets all the view types registered in the system
    /// </summary>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task GetRegisteredViews(string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(GetRegisteredViews)}");

        try
        {
            var response = await _mediator?.Send(new GetRegisteredViewsQuery())!;
            await Clients.Caller.OnGetRegisteredViewsResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(GetRegisteredViews), new Dictionary<string, string>()
            {
                {"Response", ConvertToJsonObject(response)}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
    }
    
    /// <summary>
    /// Removes a view and all links from the host
    /// </summary>
    /// <param name="viewId">The view Id to remove</param>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task RemoveView(string viewId, string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(RemoveView)}");
        try
        {
            var response = await _mediator?.Send(new RemoveViewCommand(viewId))!;
        
            _signalRUserGroups.DeRegisterUserGroupView(viewId);
        
            await Clients.Caller.OnRemoveViewResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(RemoveView), new Dictionary<string, string>()
            {
                {"ViewId", viewId},
                {"Response", ConvertToJsonObject(response)}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
    }

    /// <summary>
    /// Attaches a view to a data set
    /// </summary>
    /// <param name="viewId">The View Id to attach</param>
    /// <param name="dataSetId">The data set Id to attach to</param>
    /// <param name="ownerId">The owner Id of the view</param>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task AttachViewToDataset(string viewId, string dataSetId, string ownerId, string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(AttachViewToDataset)}");
        
        try
        {
            var response = await _mediator?.Send(new AttachViewToDataSetCommand(viewId, dataSetId, ownerId))!;
            await Clients.Caller.OnAttachViewToDatasetResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(AttachViewToDataset), new Dictionary<string, string>()
            {
                {"OwnerId", ownerId},
                {"ViewId", viewId},
                {"DataSetId", dataSetId},
                {"Response", ConvertToJsonObject(response)}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
    }
    
    /// <summary>
    /// Detaches a view from a data set
    /// </summary>
    /// <param name="viewId">The view Id to detach</param>
    /// <param name="dataSetId">The data set to detach from</param>
    /// <param name="ownerId">The owner of the view</param>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task DetachViewFromDataset(string viewId, string dataSetId, string ownerId, string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(DetachViewFromDataset)}");
        
        try
        {
            var response = await _mediator?.Send(new DetachViewFromDataSetCommand(viewId, dataSetId, ownerId))!;
            await Clients.Caller.OnDetachViewFromDatasetResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(DetachViewFromDataset), new Dictionary<string, string>()
            {
                {"OwnerId", ownerId},
                {"ViewId", viewId},
                {"DataSetId", dataSetId},
                {"Response", ConvertToJsonObject(response)}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
    }
    
    /// <summary>
    /// Sets a view's parameters
    /// </summary>
    /// <param name="viewId">The view Id to set the parameters on</param>
    /// <param name="ownerId">The owner of the view</param>
    /// <param name="viewParameters">The parameter values to set</param>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task SetViewParameters(string viewId, string ownerId, Dictionary<string, object> viewParameters, string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(SetViewParameters)}");

        try
        {
            var response = await _mediator?.Send(new SetViewParametersCommand(viewId, ownerId, viewParameters))!;
            await Clients.Caller.OnSetViewParametersResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(SetViewParameters), new Dictionary<string, string>()
            {
                {"OwnerId", ownerId},
                {"ViewId", viewId},
                {"Response", ConvertToJsonObject(response)}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
    }
    
    /// <summary>
    /// Un-Freezes a view from updates
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="ownerId">The owner of the view</param>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task SetViewAutoRefreshEnabled(string viewId, string ownerId, string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(SetViewAutoRefreshEnabled)}");
        
        try
        {
            var response = await _mediator?.Send(new SetViewEnabledCommand(viewId, ownerId))!;
            await Clients.Caller.OnSetViewAutoRefreshEnabledResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(SetViewAutoRefreshEnabled), new Dictionary<string, string>()
            {
                {"OwnerId", ownerId},
                {"ViewId", viewId},
                {"Response", ConvertToJsonObject(response)}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
    }
    
    /// <summary>
    /// Freezes a view from updates
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="ownerId">The owner of the view</param>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task SetViewAutoRefreshDisabled(string viewId, string ownerId, string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(SetViewAutoRefreshDisabled)}");

        try
        {
            var response = await _mediator?.Send(new SetViewDisabledCommand(viewId, ownerId))!;
            await Clients.Caller.OnSetViewAutoRefreshDisabledResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(SetViewAutoRefreshDisabled), new Dictionary<string, string>()
            {
                {"OwnerId", ownerId},
                {"ViewId", viewId},
                {"Response", ConvertToJsonObject(response)}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
        
      
    }
    
    /// <summary>
    /// Gets a list of active views registered for the owner
    /// </summary>
    /// <param name="ownerId">The owner Id of the views</param>
    /// <param name="callBackId">The callback Id for the response to be sent on</param>
    public async Task GetMyViews(string ownerId, string callBackId)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(GetMyViews)}");
        try
        {

            var response = await _mediator?.Send(new GetMyViewsQuery(ownerId))!;
            await Clients.Caller.OnGetMyViewsResponse(response, callBackId);

            _telemetryClient?.TrackEvent(nameof(GetMyViews), new Dictionary<string, string>()
            {
                {"OwnerId", ownerId},
                {"Response", ConvertToJsonObject(response)}
            });
        }
        catch (Exception ex)
        {
            if (operation != null) operation.Telemetry.Success = false;
            Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                "An error occured: {ErrorMessage}", ex.Message);

            throw;
        }
    }
    #endregion
    
    #region Private Methods

    private string ConvertToJsonObject(object? convertObject)
    {
        return convertObject == null ? "null" : System.Text.Json.JsonSerializer.Serialize(convertObject);
    }

    #endregion
    
}