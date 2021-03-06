using System.Collections.Concurrent;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using STAIExtensions.Host.SignalR.Client.Common;

namespace STAIExtensions.Host.SignalR.Client;

/// <summary>
/// Managed SignalR Client with auto-reconnect features and manages the send and receive of messages
/// </summary>
public class SignalRClientManaged
{

    #region Members

    private readonly HubConnection _connection;
    private readonly SignalRClientManagedOptions _options;
    private readonly ILogger<SignalRClientManaged>? _logger;
    private readonly Dictionary<string, object?> _callbackObjects = new ();
    #endregion

    #region Events

    public event OnDataSetUpdatedHandler? OnDataSetUpdated;
    public event OnDataSetViewUpdatedHandler? OnDataSetViewUpdated;
    public event OnDataViewUpdatedHandler? OnDataViewUpdated;

    #endregion
    
    #region ctor

    public SignalRClientManaged(SignalRClientManagedOptions options, ILogger<SignalRClientManaged>? logger = default)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        this._options = options;
        this._logger = logger;
        
        this._logger?.LogTrace("Starting SignalR connection on {Endpoint}", _options.Endpoint);
        
        this._connection = new HubConnectionBuilder()
            .WithUrl(this._options.Endpoint, opts =>
            {
                if (this._options.UseDefaultAuthorization)
                {
                    opts.AccessTokenProvider = () => Task.FromResult<string?>(_options.AuthBearerToken);
                    
                }
                opts.SkipNegotiation = true;
                opts.Transports = HttpTransportType.WebSockets;
            })
            .WithAutomaticReconnect()
            .Build();

        this.SetupConnectionMessages();
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Attempts to connect to the SignalR host
    /// </summary>
    public async Task ConnectAsync()
    {
        await this._connection.StartAsync();
    }

    /// <summary>
    /// Lists the available data sets on the host and executes either the success or failure actions when a
    /// response is received
    /// </summary>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task ListDataSetsAsync(
        Action<IEnumerable<Abstractions.Common.DataSetInformation>?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(ListDataSetsAsync));
            
            var callbackId = Guid.NewGuid().ToString();

            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(ListDataSets), new []{ callbackId }, cancellationToken);
        
            var result = await AwaitCallResponse<IEnumerable<Abstractions.Common.DataSetInformation>>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Lists the available data sets on the host and waits for a sync response 
    /// </summary>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<IEnumerable<Abstractions.Common.DataSetInformation>?> ListDataSets(CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(ListDataSets));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(ListDataSets), new []{ callbackId }, cancellationToken);
        
            return await AwaitCallResponse<IEnumerable<Abstractions.Common.DataSetInformation>>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Creates a new view on the host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="viewType">The view fully qualified view type name to create</param>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task CreateViewAsync(string viewType, 
        Action<ViewDetail?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(CreateViewAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(CreateView), new []{ viewType, this._options.OwnerId, callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<ViewDetail?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Creates a view on the host and waits for a sync response 
    /// </summary>
    /// <param name="viewType">The view fully qualified view type name to create</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<ViewDetail?> CreateView(string viewType, CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(CreateView));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(CreateView), new []{ viewType, this._options.OwnerId, callbackId }, cancellationToken );
        
            return await AwaitCallResponse<ViewDetail?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Retrieves a view from the host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="viewId">The view Id to retrieve</param>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task GetViewAsync(string viewId, 
        Action<ViewDetail?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(GetViewAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(GetView), new []{ viewId, this._options.OwnerId, callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<ViewDetail?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Retrieves a host view from the host and waits for a sync response 
    /// </summary>
    /// <param name="viewId">The view Id to retrieve</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<ViewDetail?> GetView(string viewId, CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(CreateView));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(GetView), new []{ viewId, this._options.OwnerId, callbackId }, cancellationToken );
        
            return await AwaitCallResponse<ViewDetail?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a list of view types that can be created from the host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task GetRegisteredViewsAsync(Action<IEnumerable<Abstractions.Common.ViewInformation>?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(GetRegisteredViewsAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(GetRegisteredViews), new []{ callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<IEnumerable<Abstractions.Common.ViewInformation>?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Retrieves a list of view types that can be created from the host and waits for a sync response 
    /// </summary>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<IEnumerable<Abstractions.Common.ViewInformation>?> GetRegisteredViews(CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(GetRegisteredViews));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(GetRegisteredViews), new []{ callbackId }, cancellationToken );
        
            return await AwaitCallResponse<IEnumerable<Abstractions.Common.ViewInformation>?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Removes a view from the Host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="viewId">The view Id to remove</param>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task RemoveViewAsync(string viewId,
        Action<bool?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(RemoveViewAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(RemoveView), new []{ viewId, callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Removes a view from the Host and waits for a sync response  
    /// </summary>
    /// <param name="viewId">The view Id to remove and unlink</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<bool?> RemoveView(string viewId, CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(RemoveView));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(RemoveView), new []{ viewId, callbackId }, cancellationToken );
        
            return await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Attaches a view to data set on the host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="dataSetId">The data set Id</param>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task AttachViewToDatasetAsync(string viewId, string dataSetId,
        Action<bool?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(AttachViewToDatasetAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(AttachViewToDataset), new []{ viewId, dataSetId, _options.OwnerId, callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Attaches a view to data set on the host and waits for a sync response  
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="dataSetId">The data set Id</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<bool?> AttachViewToDataset(string viewId, string dataSetId, CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(AttachViewToDataset));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(AttachViewToDataset), new []{ viewId, dataSetId, _options.OwnerId, callbackId }, cancellationToken );
        
            return await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Detaches a view from the data set on the host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="dataSetId">The data set Id</param>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task DetachViewFromDatasetAsync(string viewId, string dataSetId,
        Action<bool?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(DetachViewFromDatasetAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(DetachViewFromDataset), new []{ viewId, dataSetId, _options.OwnerId, callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Detaches a view from a data set on the host and waits for a sync response  
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="dataSetId">The data set Id</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<bool?> DetachViewFromDataset(string viewId, string dataSetId, CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(DetachViewFromDataset));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(DetachViewFromDataset), new []{ viewId, dataSetId, _options.OwnerId, callbackId }, cancellationToken );
        
            return await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Sets a view's parameters on the host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="viewId">The view to set the parameters on</param>
    /// <param name="parameters">The view parameter values</param>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task SetViewParametersAsync(string viewId, Dictionary<string, object>? parameters,
        Action<bool?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(SetViewParametersAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(SetViewParameters), new []{ viewId, _options.OwnerId, (object?)parameters, callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Sets a view's parameters on the host and waits for a sync response  
    /// </summary>
    /// <param name="viewId">The view Id to set the parameters on</param>
    /// <param name="parameters">The parameter values for the view</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<bool?> SetViewParameters(string viewId, Dictionary<string, object>? parameters, CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(SetViewParameters));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(SetViewParameters), new []{ viewId, _options.OwnerId, (object?)parameters, callbackId }, cancellationToken );
        
            return await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Un-Freezes a view's updates on the host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task SetViewAutoRefreshEnabledAsync(string viewId, 
        Action<bool?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(SetViewAutoRefreshEnabledAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(SetViewAutoRefreshEnabled), new []{ viewId, _options.OwnerId, callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Un-Freezes a view's updates on the host and waits for a sync response  
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<bool?> SetViewAutoRefreshEnabled(string viewId, CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(SetViewAutoRefreshEnabled));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(SetViewAutoRefreshEnabled), new []{ viewId, _options.OwnerId, callbackId }, cancellationToken );
        
            return await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Freezes a view's updates on the host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task SetViewAutoRefreshDisabledAsync(string viewId, 
        Action<bool?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(SetViewAutoRefreshDisabledAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(SetViewAutoRefreshDisabled), new []{ viewId, _options.OwnerId, callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Freezes a view's updates on the host and waits for a sync response
    /// </summary>
    /// <param name="viewId">The view Id</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<bool?> SetViewAutoRefreshDisabled(string viewId, CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(SetViewAutoRefreshDisabled));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(SetViewAutoRefreshDisabled), new []{ viewId, _options.OwnerId, callbackId }, cancellationToken );
        
            return await AwaitCallResponse<bool?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Gets a list of owner views on the host and executes either the success or failure actions when a response is returned
    /// </summary>
    /// <param name="success">The success action</param>
    /// <param name="error">The error action</param>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    public async Task GetMyViewsAsync(
        Action<IEnumerable<Abstractions.Common.MyViewInformation>?> success, 
        Action<Exception>? error = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(GetMyViewsAsync));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(GetMyViews), new []{ _options.OwnerId, callbackId }, cancellationToken );
        
            var result = await AwaitCallResponse<IEnumerable<Abstractions.Common.MyViewInformation>?>(callbackId, cancellationToken);
            
            success.Invoke(result);
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            error?.Invoke(ex);
        }
    }
    
    /// <summary>
    /// Gets a list of owner views on the host and waits for a sync response
    /// </summary>
    /// <param name="cancellationToken">Timeout cancellation token</param>
    /// <returns></returns>
    public async Task<IEnumerable<Abstractions.Common.MyViewInformation>?> GetMyViews(CancellationToken cancellationToken = default)
    {
        try
        {
            this._logger?.LogTrace("Starting operation {OperationName}", nameof(GetMyViews));
            
            var callbackId = Guid.NewGuid().ToString();
            
            this.InitialiseCallbackObjectResult(callbackId);

            await _connection?.SendCoreAsync(nameof(GetMyViews), new []{ _options.OwnerId, callbackId }, cancellationToken );
        
            return await AwaitCallResponse<IEnumerable<Abstractions.Common.MyViewInformation>?>(callbackId, cancellationToken);
            
        }
        catch (Exception ex)
        {
            this._logger?.LogError(ex, "An error occured: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    #endregion
    
    #region Private Methods

    private void SetupConnectionMessages()
    {
        _connection.On<string>("OnDataSetUpdated", (dataSetId) =>
        {
            this.OnDataSetUpdated?.Invoke(this, dataSetId);
        });
        
        _connection.On<string>("OnDataSetViewUpdated", (viewId) =>
        {
            this.OnDataSetViewUpdated?.Invoke(this, viewId);

            if (OnDataViewUpdated != null)
            {
                Task.Run(async () =>
                {
                    var viewDetail = await GetView(viewId);
                    if (viewDetail != null)
                        OnDataViewUpdated.Invoke(this, viewDetail);
                });
            }
            
        });
        
        _connection.On<IEnumerable<Abstractions.Common.DataSetInformation>, string>("OnListDataSetsResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, result);
        });
        
        _connection.On<object, string>("OnDataSetViewCreated", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, new ViewDetail(result?.ToString()));
        });
        
        _connection.On<object, string>("OnGetViewResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, new ViewDetail(result?.ToString()));
        });
        
        _connection.On<IEnumerable<Abstractions.Common.ViewInformation>, string>("OnGetRegisteredViewsResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, result);
        });
        
        _connection.On<bool, string>("OnRemoveViewResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, result);
        });
        
        _connection.On<bool, string>("OnAttachViewToDatasetResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, result);
        });
        
        _connection.On<bool, string>("OnDetachViewFromDatasetResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, result);
        });
        
        _connection.On<bool, string>("OnSetViewParametersResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, result);
        });
        
        _connection.On<bool, string>("OnSetViewAutoRefreshEnabledResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, result);
        });
        
        _connection.On<bool, string>("OnSetViewAutoRefreshDisabledResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, result);
        });
        
        _connection.On<IEnumerable<Abstractions.Common.MyViewInformation>, string>("OnGetMyViewsResponse", (result, callbackId) =>
        {
            this.PushCallbackObjectResult(callbackId, result);
        });
    }

    private void InitialiseCallbackObjectResult(string callbackId)
    {
        try
        {
            if (string.IsNullOrEmpty(callbackId) || callbackId.Trim() == "")
                throw new ArgumentNullException(nameof(callbackId));

            lock (_callbackObjects)
            {
                _callbackObjects[callbackId] = new EmtpyObject();
            }
        }
        catch (Exception e)
        {
            this._logger?.LogError(e, "Could not set the Callback object result: {ErrorMessage}", e.Message);
            throw;
        }
    }

    private void PushCallbackObjectResult(string callbackId, object? result)
    {
        try
        {
            if (string.IsNullOrEmpty(callbackId) || callbackId.Trim() == "")
                throw new ArgumentNullException(nameof(callbackId));

            lock (_callbackObjects)
            {
                _callbackObjects[callbackId] = result;
            }
        }
        catch (Exception e)
        {
            this._logger?.LogError(e, "Could not set the Callback object result: {ErrorMessage}", e.Message);
            throw;
        }
    }

    private object? GetCallbackObjectResult(string callbackId)
    {
        try
        {
            if (string.IsNullOrEmpty(callbackId) || callbackId.Trim() == "")
                throw new ArgumentNullException(nameof(callbackId));
            
            object? result;
            lock (_callbackObjects)
            {
                if (!_callbackObjects.ContainsKey(callbackId))
                    throw new Exception($"Callback with Id {callbackId} does not exist");

                result = _callbackObjects[callbackId];
            }

            return result;
        }
        catch (Exception e)
        {
            this._logger?.LogError(e, "Could not get the Callback object result: {ErrorMessage}", e.Message);
            throw;
        }
    }

    private void RemoveCallbackObjectResult(string callbackId)
    {
        try
        {
            if (string.IsNullOrEmpty(callbackId) || callbackId.Trim() == "")
                throw new ArgumentNullException(nameof(callbackId));
            
            object? result;
            lock (_callbackObjects)
            {
                if (_callbackObjects.ContainsKey(callbackId))
                    _callbackObjects.Remove(callbackId);

            }
        }
        catch (Exception e)
        {
            this._logger?.LogError(e, "Could not clear the Callback object result: {ErrorMessage}", e.Message);
            throw;
        }
    }
    
    private async Task<T?> AwaitCallResponse<T>(string callbackId, CancellationToken cancellationToken = default)
    {

        var result = default(T);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var callbackResultObject = GetCallbackObjectResult(callbackId);
            if (!(callbackResultObject is EmtpyObject))
            {
                RemoveCallbackObjectResult(callbackId);
                
                result = (T?)callbackResultObject;
                break;
            }

            await Task.Delay(100, cancellationToken);
        }
        return result;
    }
    #endregion

    
}