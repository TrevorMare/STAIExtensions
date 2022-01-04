using Grpc.Net.Client;

namespace STAIExtensions.Host.Grpc.Client;

public class GrpcClientManaged : IDisposable
{

    #region Events

    public event OnDataSetUpdatedHandler? OnDataSetUpdated;
    public event OnDataSetViewUpdatedHandler? OnDataSetViewUpdated;
    public event OnDataSetViewUpdatedJsonHandler? OnDataSetViewUpdatedJson;
   
    #endregion
    
    #region Members

    private bool _disposed = false;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly GrpcChannel? _aiExtensionsChannel;
    private readonly STAIExtensionsGrpcService.STAIExtensionsGrpcServiceClient _client;
    private readonly GrpcClientManagedOptions _options;
    #endregion

    #region ctor

    public GrpcClientManaged(GrpcClientManagedOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        
        if (_options.UseHttp2UnencryptedSupport)
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        this._aiExtensionsChannel = GrpcChannel.ForAddress(_options.ChannelUrl, _options.GrpcChannelOptions);
        this._client = new STAIExtensionsGrpcService.STAIExtensionsGrpcServiceClient(this._aiExtensionsChannel);

        this.SetupAsyncTasks();
    }
    #endregion

    #region Public Methods

    public async Task<IDataSetView?> CreateViewAsync(string viewTypeName)
    {
        if (string.IsNullOrEmpty(viewTypeName) || viewTypeName.Trim() == "")
            throw new ArgumentNullException(nameof(viewTypeName));
        
        var response = await this._client.CreateViewAsync(new CreateViewRequest()
        {
            OwnerId = _options.OwnerId,
            ViewType = viewTypeName
        }, null, null, _cancellationTokenSource.Token);
        return response;
    }

    public async Task<IDataSetView> GetViewAsync(string viewId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        var response = await this._client.GetViewAsync(new GetViewRequest()
        {
            OwnerId = _options.OwnerId,
            ViewId = viewId
        }, null, null, _cancellationTokenSource.Token);
        return response;
    }
    
    public async Task<string?> GetViewJsonAsync(string viewId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        var response = await this._client.GetViewJsonAsync(new GetViewRequest()
        {
            OwnerId = _options.OwnerId,
            ViewId = viewId
        }, null, null, _cancellationTokenSource.Token);
        return response?.Payload;
    }
    
    public async Task<IEnumerable<DataSetInformation>?> ListDataSetsAsync()
    {
        var response = await this._client.ListDataSetsAsync(new NoParameters(), 
            null, null, _cancellationTokenSource.Token);
        return response.Items.ToList();
    }
    
    public async Task<IEnumerable<DataSetViewInformation>?> GetRegisteredViewsAsync()
    {
        var response = await this._client.GetRegisteredViewsAsync(new NoParameters(), 
            null, null, _cancellationTokenSource.Token);
        return response.Items.ToList();
    }
    
    public async Task<bool> RemoveViewAsync(string viewId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        var response = await this._client.RemoveViewAsync(new RemoveViewRequest()
        {
            OwnerId = _options.OwnerId,
            ViewId = viewId
        },null, null, _cancellationTokenSource.Token);
        return response.Result;
    }
    
    public async Task<bool> AttachViewToDatasetAsync(string viewId, string dataSetId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        if (string.IsNullOrEmpty(dataSetId) || dataSetId.Trim() == "")
            throw new ArgumentNullException(nameof(dataSetId));
        
        var response = await this._client.AttachViewToDatasetAsync(new AttachViewToDatasetRequest()
        {
            OwnerId = _options.OwnerId,
            ViewId = viewId,
            DataSetId = dataSetId
        },null, null, _cancellationTokenSource.Token);
        return response.Result;
    }
    
    public async Task<bool> DetachViewFromDatasetAsync(string viewId, string dataSetId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        if (string.IsNullOrEmpty(dataSetId) || dataSetId.Trim() == "")
            throw new ArgumentNullException(nameof(dataSetId));
        
        var response = await this._client.DetachViewFromDatasetAsync(new DetachViewFromDatasetRequest()
        {
            OwnerId = _options.OwnerId,
            ViewId = viewId,
            DataSetId = dataSetId
        },null, null, _cancellationTokenSource.Token);
        return response.Result;
    }
    
    public async Task<bool> SetViewParameters(string viewId, string dataSetId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        if (string.IsNullOrEmpty(dataSetId) || dataSetId.Trim() == "")
            throw new ArgumentNullException(nameof(dataSetId));
        
        var response = await this._client.DetachViewFromDatasetAsync(new DetachViewFromDatasetRequest()
        {
            OwnerId = _options.OwnerId,
            ViewId = viewId,
            DataSetId = dataSetId
        },null, null, _cancellationTokenSource.Token);
        return response.Result;
    }

    public async Task<bool> SetViewAutoRefreshEnabledAsync(string viewId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        var response = await this._client.SetViewAutoRefreshEnabledAsync(new SetViewAutoRefreshEnabledRequest()
        {
            OwnerId = _options.OwnerId,
            ViewId = viewId
        },null, null, _cancellationTokenSource.Token);
        return response.Result;
    }
    
    public async Task<bool> SetViewAutoRefreshDisabledAsync(string viewId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        var response = await this._client.SetViewAutoRefreshDisabledAsync(new SetViewAutoRefreshDisabledRequest()
        {
            OwnerId = _options.OwnerId,
            ViewId = viewId
        },null, null, _cancellationTokenSource.Token);
        return response.Result;
    }
    
    #endregion
    
    #region Private Methods
    private void SetupAsyncTasks()
    {
        Task.Run(async () =>
        {
            var asyncServerCall =
                this._client.OnDataSetUpdated(new NoParameters(), null, null, _cancellationTokenSource.Token);
            while (await asyncServerCall.ResponseStream.MoveNext(_cancellationTokenSource.Token))
            {
                OnDataSetUpdated?.Invoke(this, asyncServerCall.ResponseStream.Current.DataSetId);
            }
        });

        Task.Run(async () =>
        {
            var asyncServerCall =
                this._client.OnDataViewUpdated(new OnDataSetViewUpdatedRequest() {OwnerId = _options.OwnerId}, null,
                    null, _cancellationTokenSource.Token);
            while (await asyncServerCall.ResponseStream.MoveNext(_cancellationTokenSource.Token))
            {
                string viewId = asyncServerCall.ResponseStream.Current.ViewId;
                
                OnDataSetViewUpdated?.Invoke(this, viewId);

                if (OnDataSetViewUpdatedJson != null)
                {
                    var jsonResponse = await _client.GetViewJsonAsync(
                        new GetViewRequest() {OwnerId = _options.OwnerId, ViewId = viewId}, null, null,
                        _cancellationTokenSource.Token);
                    OnDataSetViewUpdatedJson.Invoke(this,
                        new DataSetViewUpdatedJsonParams(viewId, jsonResponse.Payload));
                }
                
            }
        });
    }
    #endregion

    #region Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _disposed == false)
        {
            _disposed = true;
            this._cancellationTokenSource.Cancel();
            this._aiExtensionsChannel?.ShutdownAsync();
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion
    
}