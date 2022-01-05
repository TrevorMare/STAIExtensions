using System.Text.Json;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using STAIExtensions.Host.Grpc.Client.Common;

namespace STAIExtensions.Host.Grpc.Client;

public class GrpcClientManaged : IDisposable
{

    #region Events
    public event OnDataSetUpdatedHandler? OnDataSetUpdated;
    public event OnDataSetViewUpdatedHandler? OnDataSetViewUpdated;
    public event OnDataSetViewUpdatedJsonHandler? OnDataSetViewUpdatedJson;
    public event OnConnectionStateChangedHandler? OnConnectionStateChanged;
  
    #endregion
    
    #region Members
    private bool _disposed = false;
    private DateTime? _serverUtcDateTime = null;
    private DateTime _lastConnectionAttempt = DateTime.Now.ToUniversalTime();
    private int _connectionAttemptNumber = 0;
    private ConnectionState _connectionState = ConnectionState.Closed;
    
    protected readonly GrpcClientManagedOptions Options;
    protected readonly ILogger<GrpcClientManaged>? Logger;
    
    protected GrpcChannel? AIExtensionsChannel;
    protected STAIExtensionsGrpcService.STAIExtensionsGrpcServiceClient? Client;
    protected CancellationTokenSource CancellationTokenSource = new();
    #endregion

    #region Properties
    public Common.ConnectionState ConnectionState
    {
        get => _connectionState;
        set
        {
            if (_connectionState != value)
            {
                _connectionState = value;
                OnConnectionStateChanged?.Invoke(this, _connectionState);
            }
        }
    }

    protected int? AutoReconnectMaxAttempts => Options.AutoReconnectMaxAttempts;
    
    protected bool AutoReconnect => Options.AutoReconnect;
    #endregion
    
    #region ctor

    public GrpcClientManaged(GrpcClientManagedOptions options, ILogger<GrpcClientManaged>? logger = default)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        Logger = logger;

        if (Options.UseHttp2UnencryptedSupport)
        {
            Logger?.LogTrace("Setting Http2UnencryptedSupport option on connection");
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }

        CreateConnectionAndClient();
    }
    #endregion

    #region Public Methods

    public async Task<IDataSetView?> CreateViewAsync(string viewTypeName)
    {
        try
        {
            if (string.IsNullOrEmpty(viewTypeName) || viewTypeName.Trim() == "")
                throw new ArgumentNullException(nameof(viewTypeName));

            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.CreateViewAsync(new CreateViewRequest()
            {
                OwnerId = Options.OwnerId,
                ViewType = viewTypeName
            }, null, null, CancellationTokenSource.Token);
            return response;
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }

    public async Task<IDataSetView?> GetViewAsync(string viewId)
    {
        try
        {
            if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
                throw new ArgumentNullException(nameof(viewId));
        
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.GetViewAsync(new GetViewRequest()
            {
                OwnerId = Options.OwnerId,
                ViewId = viewId
            }, null, null, CancellationTokenSource.Token);
            return response;
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    public async Task<string?> GetViewJsonAsync(string viewId)
    {
        try
        {
            if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
                throw new ArgumentNullException(nameof(viewId));
        
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.GetViewJsonAsync(new GetViewRequest()
            {
                OwnerId = Options.OwnerId,
                ViewId = viewId
            }, null, null, CancellationTokenSource.Token);
            return response?.Payload;
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    public async Task<IEnumerable<DataSetInformation>?> ListDataSetsAsync()
    {
        try
        {
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.ListDataSetsAsync(new NoParameters(), 
                null, null, CancellationTokenSource.Token);
            return response.Items.ToList();
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    public async Task<IEnumerable<DataSetViewInformation>?> GetRegisteredViewsAsync()
    {
        try
        {
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.GetRegisteredViewsAsync(new NoParameters(), 
                null, null, CancellationTokenSource.Token);
            return response.Items.ToList();
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    public async Task<bool?> RemoveViewAsync(string viewId)
    {
        try
        {
            if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
                throw new ArgumentNullException(nameof(viewId));
     
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.RemoveViewAsync(new RemoveViewRequest()
            {
                OwnerId = Options.OwnerId,
                ViewId = viewId
            },null, null, CancellationTokenSource.Token);
            return response.Result;
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    public async Task<bool?> AttachViewToDatasetAsync(string viewId, string dataSetId)
    {
        try
        {
            if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
                throw new ArgumentNullException(nameof(viewId));
        
            if (string.IsNullOrEmpty(dataSetId) || dataSetId.Trim() == "")
                throw new ArgumentNullException(nameof(dataSetId));
        
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.AttachViewToDatasetAsync(new AttachViewToDatasetRequest()
            {
                OwnerId = Options.OwnerId,
                ViewId = viewId,
                DataSetId = dataSetId
            },null, null, CancellationTokenSource.Token);
            return response.Result;
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    public async Task<bool?> DetachViewFromDatasetAsync(string viewId, string dataSetId)
    {
        try
        {
            if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
                throw new ArgumentNullException(nameof(viewId));
        
            if (string.IsNullOrEmpty(dataSetId) || dataSetId.Trim() == "")
                throw new ArgumentNullException(nameof(dataSetId));
        
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.DetachViewFromDatasetAsync(new DetachViewFromDatasetRequest()
            {
                OwnerId = Options.OwnerId,
                ViewId = viewId,
                DataSetId = dataSetId
            },null, null, CancellationTokenSource.Token);
            return response.Result;
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    public async Task<bool?> SetViewParameters(string viewId, Dictionary<string, object>? viewParameters)
    {
        try
        {
            if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
                throw new ArgumentNullException(nameof(viewId));

            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var jsonPayload = string.Empty;
            if (viewParameters != null)
                jsonPayload = JsonSerializer.Serialize(viewParameters);
        
            var response = await this.Client.SetViewParametersAsync(new SetViewParametersRequest() 
            {
                OwnerId = Options.OwnerId,
                ViewId = viewId,
                JsonPayload = jsonPayload
            },null, null, CancellationTokenSource.Token);
        
            return response.Result;
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }

    public async Task<bool?> SetViewAutoRefreshEnabledAsync(string viewId)
    {
        try
        {
            if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
                throw new ArgumentNullException(nameof(viewId));
        
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.SetViewAutoRefreshEnabledAsync(new SetViewAutoRefreshEnabledRequest()
            {
                OwnerId = Options.OwnerId,
                ViewId = viewId
            },null, null, CancellationTokenSource.Token);
            return response.Result;
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    public async Task<bool?> SetViewAutoRefreshDisabledAsync(string viewId)
    {
        try
        {
            if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
                throw new ArgumentNullException(nameof(viewId));
        
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.SetViewAutoRefreshDisabledAsync(new SetViewAutoRefreshDisabledRequest()
            {
                OwnerId = Options.OwnerId,
                ViewId = viewId
            },null, null, CancellationTokenSource.Token);
            return response.Result;
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    public async Task<List<MyViewResponse.Types.MyView>?> GetMyViewsAsync()
    {
        try
        {
            if (this.ConnectionState != ConnectionState.Connected)
            {
                this?.Logger.LogCritical("Connection state is {ConnectionState} and cannot continue", this.ConnectionState);
                return null;
            }
        
            var response = await this.Client.GetMyViewsAsync(new GetMyViewsRequest() 
            {
                OwnerId = Options.OwnerId
            },null, null, CancellationTokenSource.Token);
            return response.Items.ToList();
        }
        catch (RpcException e) when(e.StatusCode == StatusCode.Unavailable)
        {
            this.Logger?.LogCritical("Connection lost, will attempt reconnect");
            this.ConnectionState = ConnectionState.Closed;
            return null;
        }
    }
    
    #endregion
    
    #region Private Methods

    /// <summary>
    /// Instantiates the connection and the client object
    /// </summary>
    private void CreateConnectionAndClient()
    {
        try
        {
            this._lastConnectionAttempt = DateTime.Now.ToUniversalTime();
            
            this.ConnectionState = ConnectionState.Connecting;
        
            this.AIExtensionsChannel = CreateChannel();
            this.Client = CreateClient(this.AIExtensionsChannel);
        
            this.SetupAsyncTasks();

        }
        catch (Exception e)
        {
            this.ConnectionState = ConnectionState.Failed;
            Logger?.LogError(e, "An error occured connecting to the server {Error}", e);
            throw;
        }
    }

    /// <summary>
    /// Closes the connection and the Client
    /// </summary>
    private async Task CloseConnectionAndClient()
    {
        this.Client = null;
        if (this.AIExtensionsChannel != null)
        {
            await this.AIExtensionsChannel.ShutdownAsync();
            this.AIExtensionsChannel.Dispose();
        }
        this.ConnectionState = ConnectionState.Closed;
        this.CancellationTokenSource = new();
    }
    
    /// <summary>
    /// Sets up the Async Tasks
    /// </summary>
    private void SetupAsyncTasks()
    {
        Task.Run(async () =>
        {
            Logger?.LogInformation("Setting up Connection Monitor");
            
            // Set up a timer task to monitor the connection for a connection failure
            while (CancellationTokenSource.Token.IsCancellationRequested == false)
            {
                if (_serverUtcDateTime.HasValue)
                {
                    if (_serverUtcDateTime < DateTime.Now.ToUniversalTime().AddSeconds(-20))
                    {
                        this.Logger?.LogCritical("Connection to service lost, Attempting Reconnect");
                        this.ReConnect();
                        return;
                    }
                }
                else
                {
                    if (_lastConnectionAttempt < DateTime.Now.ToUniversalTime().AddSeconds(-20))
                    {
                        this.Logger?.LogCritical("Connection to service could not be made, Attempting Reconnect");
                        this.ReConnect();
                        return;
                    }
                }
                await Task.Delay(1000);
            }
        });
        
        if (this.Client == null) return;
        
        Task.Run(async () =>
        {
            Logger?.LogInformation("Register Connection State Listener");
            // Setup the Connection State Listener
            var asyncServerCall =
                this.Client.RegisterConnectionState(new NoParameters(), null, null, CancellationTokenSource.Token);
            
            // While we have not yet cancelled
            while (await asyncServerCall.ResponseStream.MoveNext(CancellationTokenSource.Token))
            {
                this._connectionAttemptNumber = 0;
                this.ConnectionState = ConnectionState.Connected;
                _serverUtcDateTime = asyncServerCall.ResponseStream.Current.ServerTimeUTC.ToDateTime();
            }
        });
        
        
        Task.Run(async () =>
        {
            Logger?.LogInformation("Register DataSet updated callback");
            // Set up the DataSet updated callback
            var asyncServerCall =
                this.Client.OnDataSetUpdated(new NoParameters(), null, null, CancellationTokenSource.Token);
            
            // While we have not yet cancelled
            while (await asyncServerCall.ResponseStream.MoveNext(CancellationTokenSource.Token))
            {
                OnDataSetUpdated?.Invoke(this, asyncServerCall.ResponseStream.Current.DataSetId);
            }
        });

        Task.Run(async () =>
        {
            Logger?.LogInformation("Register View updated callback");
            // Set up the View Updated Callback
            var asyncServerCall =
                this.Client.OnDataViewUpdated(new OnDataSetViewUpdatedRequest() {OwnerId = Options.OwnerId}, null,
                    null, CancellationTokenSource.Token);
            
            // While we have not yet cancelled
            while (await asyncServerCall.ResponseStream.MoveNext(CancellationTokenSource.Token))
            {
                // Raise the Data Set View Updated Event
                string viewId = asyncServerCall.ResponseStream.Current.ViewId;
                OnDataSetViewUpdated?.Invoke(this, viewId);

                if (OnDataSetViewUpdatedJson == null) continue;

                var jsonResponse = await GetViewJsonAsync(viewId);
                if (string.IsNullOrEmpty(jsonResponse)) continue;
                // Raise the On Data Set View Updated with the json payload
                OnDataSetViewUpdatedJson.Invoke(this,
                    new DataSetViewUpdatedJsonParams(viewId, jsonResponse));

            }
        });
    }
    #endregion

    #region Protected Methods
    
    /// <summary>
    /// Forcibly closes the connections and reconnects the client and channel
    /// </summary>
    protected void ReConnect()
    {
        
        if (AutoReconnect == false)
            throw new Exception("Connection to service lost");
        if (AutoReconnectMaxAttempts.HasValue && AutoReconnectMaxAttempts.Value > _connectionAttemptNumber)
            throw new Exception("Maximum number of reconnect attempts reached");
        else
            _connectionAttemptNumber++;
        
        this.ConnectionState = ConnectionState.Reconnecting;
        this.CancellationTokenSource.Token.Register(() =>
        {
            // Give a moment for the cancellation to take place
            Task.Delay(1000);
            CloseConnectionAndClient().ConfigureAwait(false);
            this._serverUtcDateTime = null;
            CreateConnectionAndClient();
        });
        this.CancellationTokenSource.Cancel();
    }

    /// <summary>
    /// Creates a Grpc Channel
    /// </summary>
    /// <returns></returns>
    protected virtual GrpcChannel CreateChannel()
    {
        var defaultCredentials = GetChannelCredentials();
        if (defaultCredentials != null)
        {
            this.Logger?.LogInformation("Setting the credentials on the options for the channel");
            // Override the credentials on the channel
            Options.GrpcChannelOptions.Credentials =
                ChannelCredentials.Create(new SslCredentials(), defaultCredentials);
        }
       
        var channel = GrpcChannel.ForAddress(Options.ChannelUrl, Options.GrpcChannelOptions);
        return channel;
    }

    /// <summary>
    /// Creates a new Client
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    protected virtual STAIExtensionsGrpcService.STAIExtensionsGrpcServiceClient CreateClient(GrpcChannel channel)
    {
        return new STAIExtensionsGrpcService.STAIExtensionsGrpcServiceClient(channel);
    }

    /// <summary>
    /// Checks if the options specifies that we should use the default credentials and
    /// then sets up the authorization for the channel
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    protected virtual CallCredentials? GetChannelCredentials()
    {
        try
        {
            if (Options.UseDefaultAuthorization != true) return null;
        
            if (string.IsNullOrEmpty(Options.AuthBearerToken))
                throw new Exception("The Auth Bearer Token is required when use default credentials is set");
       
            this.Logger.LogInformation("Setting up Authorization Bearer Token on the Grpc Channel");
            
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(Options.AuthBearerToken))
                {
                    metadata.Add("Authorization", $"Bearer {Options.AuthBearerToken}");
                }
                return Task.CompletedTask;
            });

            return credentials;
        }
        catch (Exception e)
        {
            Logger?.LogError(e, "An Error occured setting up the Authorization Credentials {Error}", e);
            throw;
        }
    }
    #endregion
    
    #region Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _disposed == false)
        {
            _disposed = true;
            this.CancellationTokenSource.Cancel();
            this.AIExtensionsChannel?.ShutdownAsync();
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion
    
}