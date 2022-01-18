using System.Reflection;
using System.Text.Json;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.CQRS.DataSets.Queries;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Host.Grpc.Mapping;

namespace STAIExtensions.Host.Grpc.Services;

/// <summary>
/// STAIExtension Grpc Host Service that interacts with the collections of Data Sets and Views.
/// This host exposes streams that will notify the client on connection state and updates that occured
/// in the Data Sets and Views
/// </summary>
public class STAIExtensionsGrpcService : STAIExtensions.Host.Grpc.STAIExtensionsGrpcService.STAIExtensionsGrpcServiceBase
{

    #region Members

    private readonly ILogger<STAIExtensionsGrpcService>? _logger;
    private readonly IMediator? _mediator;
    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;
    private readonly TelemetryClient? _telemetryClient;

    private readonly Queue<string> _viewUpdates = new();
    private readonly Queue<string> _datasetUpdates = new();

    #endregion

    #region ctor

    public STAIExtensionsGrpcService()
    {
        _mediator = Abstractions.DependencyExtensions.Mediator;
        _viewCollection = Abstractions.DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>()!;
        _dataSetCollection =
            Abstractions.DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>()!;
        _logger = Abstractions.DependencyExtensions.CreateLogger<STAIExtensionsGrpcService>();
        _telemetryClient = Abstractions.DependencyExtensions.TelemetryClient;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Connects to a stream that enables the connection to be monitored by the client, this
    /// method wil periodically send the server UTC Date time to the client. This allows
    /// the client to monitor if the Grpc Connection is still alive.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    public override async Task RegisterConnectionState(NoParameters request,
        IServerStreamWriter<ConnectionStateResponse> responseStream, ServerCallContext context)
    {
        _telemetryClient?.TrackEvent($"Register_{nameof(RegisterConnectionState)}");

        var contextCancellationToken = context.CancellationToken;
        while (contextCancellationToken.IsCancellationRequested == false)
        {
            await responseStream.WriteAsync(new ConnectionStateResponse()
                {
                    ServerTimeUTC = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime())
                }
            );
            _telemetryClient?.TrackEvent($"{nameof(RegisterConnectionState)}");
            await Task.Delay(1000, contextCancellationToken);
        }
    }

    /// <summary>
    /// Connects a stream to client that will notify it that a Data Set has been updated. 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    public override async Task OnDataSetUpdated(NoParameters request,
        IServerStreamWriter<OnDataSetUpdatedResponse> responseStream, ServerCallContext context)
    {
        _telemetryClient?.TrackEvent($"Register_{nameof(OnDataSetUpdated)}");

        _dataSetCollection.OnDataSetUpdated += (sender, args) => { this._datasetUpdates.Enqueue(sender.DataSetId); };

        var contextCancellationToken = context.CancellationToken;

        while (contextCancellationToken.IsCancellationRequested == false)
        {
            while (this._datasetUpdates.Count > 0)
            {
                string dataSetId = this._datasetUpdates.Dequeue();

                _telemetryClient?.TrackEvent($"{nameof(OnDataViewUpdated)}", new Dictionary<string, string>()
                {
                    {"DataSetId", dataSetId}
                });

                await responseStream.WriteAsync(new OnDataSetUpdatedResponse() {DataSetId = dataSetId});
            }

            await Task.Delay(200, contextCancellationToken);
        }
    }

    /// <summary>
    /// Connects a stream to client that will notify it that a Data View has been updated. 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    public override async Task OnDataViewUpdated(OnDataSetViewUpdatedRequest request,
        IServerStreamWriter<OnDataSetViewUpdatedResponse> responseStream, ServerCallContext context)
    {
        _telemetryClient?.TrackEvent($"Register_{nameof(OnDataViewUpdated)}", new Dictionary<string, string>()
        {
            {"OwnerId", request.OwnerId},
        });

        _viewCollection.OnDataSetViewUpdated += (sender, args) =>
        {
            if (string.Equals(sender?.OwnerId?.Trim(), request.OwnerId.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                this._viewUpdates.Enqueue(sender.Id);
            }
        };

        var contextCancellationToken = context.CancellationToken;

        while (contextCancellationToken.IsCancellationRequested == false)
        {
            while (this._viewUpdates.Count > 0)
            {
                string viewId = this._viewUpdates.Dequeue();

                _telemetryClient?.TrackEvent($"{nameof(OnDataViewUpdated)}", new Dictionary<string, string>()
                {
                    {"OwnerId", request.OwnerId},
                    {"ViewId", viewId}
                });

                await responseStream.WriteAsync(new OnDataSetViewUpdatedResponse() {ViewId = viewId});
            }

            await Task.Delay(200, contextCancellationToken);
        }
    }

    /// <summary>
    /// Creates a new view and assigns it to the owner
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<IDataSetView> CreateView(CreateViewRequest request, ServerCallContext context)
    {

        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(CreateView)}");
        try
        {
            var response = await _mediator?.Send(new CreateViewCommand(request.ViewType, request.OwnerId))!;

            _telemetryClient?.TrackEvent(nameof(CreateView), new Dictionary<string, string>()
            {
                {"ViewId", request.ViewType},
                {"OwnerId", request.OwnerId},
                {"Success", (response != null).ToString()}
            });

            return ModelMapper.ConvertViewToDataSetViewResponse(response);
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
    /// Returns the View Json data to be deserialized on the client. This return object contains
    /// the type to assist with this
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<IDataSetViewJson> GetViewJson(GetViewRequest request, ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(GetViewJson)}");
        try
        {
            var response = await _mediator?.Send(new GetDataViewQuery(request.ViewId, request.OwnerId))!;

            _telemetryClient?.TrackEvent(nameof(GetViewJson), new Dictionary<string, string>()
            {
                {"ViewId", request.ViewId},
                {"OwnerId", request.OwnerId}
            });

            if (response == null) return new IDataSetViewJson();

            return new IDataSetViewJson()
            {
                Payload = ModelMapper.ConvertViewToJson(response)
            };
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
    /// Gets the interface exposed properties of the View. It does not return the full view, to retrieve
    /// the full view data use the <see cref="GetViewJson"/> method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<IDataSetView> GetView(GetViewRequest request, ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(GetView)}");
        try
        {
            var response = await _mediator?.Send(new GetDataViewQuery(request.ViewId, request.OwnerId))!;

            _telemetryClient?.TrackEvent(nameof(GetView), new Dictionary<string, string>()
            {
                {"ViewId", request.ViewId},
                {"OwnerId", request.OwnerId}
            });

            return ModelMapper.ConvertViewToDataSetViewResponse(response);
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
    /// Lists all the data sets registered that can be attached to
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ListDataSetsResponse> ListDataSets(NoParameters request, ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(ListDataSets)}");
        try
        {
            var response = await _mediator?.Send(new ListDataSetsQuery())!;

            _telemetryClient?.TrackEvent(nameof(ListDataSets), new Dictionary<string, string>()
            {
                {"Response", ConvertToJsonObject(response)}
            });

            return ModelMapper.ConvertDataSetInformation(response);
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
    /// Gets a list of registered views that can be used to create a client instance of
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<GetRegisteredViewsResponse> GetRegisteredViews(NoParameters request,
        ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>(
                $"{this.GetType().Name} - {nameof(GetRegisteredViews)}");
        try
        {
            var response = await _mediator?.Send(new GetRegisteredViewsQuery())!;

            _telemetryClient?.TrackEvent(nameof(GetRegisteredViews), new Dictionary<string, string>()
            {
                {"Response", ConvertToJsonObject(response)}
            });

            return ModelMapper.ConvertViewInformationRegisteredViewsResponse(response);
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
    /// Removes a view and all linking from the collection managers
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<BoolResponse> RemoveView(RemoveViewRequest request, ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(RemoveView)}");
        try
        {
            var response = await _mediator?.Send(new RemoveViewCommand(request.ViewId))!;

            _telemetryClient?.TrackEvent(nameof(RemoveView), new Dictionary<string, string>()
            {
                {"OwnerId", request.OwnerId},
                {"ViewId", request.ViewId},
                {"Response", ConvertToJsonObject(response)}
            });

            return new BoolResponse() {Result = response};
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
    /// Attaches a view to a Data Set for updates
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<BoolResponse> AttachViewToDataset(AttachViewToDatasetRequest request,
        ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>(
                $"{this.GetType().Name} - {nameof(AttachViewToDataset)}");
        try
        {
            var response =
                await _mediator?.Send(
                    new AttachViewToDataSetCommand(request.ViewId, request.DataSetId, request.OwnerId))!;

            _telemetryClient?.TrackEvent(nameof(AttachViewToDataset), new Dictionary<string, string>()
            {
                {"OwnerId", request.OwnerId},
                {"ViewId", request.ViewId},
                {"DataSetId", request.DataSetId},
                {"Response", ConvertToJsonObject(response)}
            });

            return new BoolResponse() {Result = response};
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
    /// Detaches a view from a Data Set
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<BoolResponse> DetachViewFromDataset(DetachViewFromDatasetRequest request,
        ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>(
                $"{this.GetType().Name} - {nameof(DetachViewFromDataset)}");
        try
        {
            var response =
                await _mediator?.Send(new DetachViewFromDataSetCommand(request.ViewId, request.DataSetId,
                    request.OwnerId))!;

            _telemetryClient?.TrackEvent(nameof(DetachViewFromDataset), new Dictionary<string, string>()
            {
                {"OwnerId", request.OwnerId},
                {"ViewId", request.ViewId},
                {"DataSetId", request.DataSetId},
                {"Response", ConvertToJsonObject(response)}
            });

            return new BoolResponse() {Result = response};
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
    /// Freezes updates from data sets on a View
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<BoolResponse> SetViewAutoRefreshDisabled(SetViewAutoRefreshDisabledRequest request,
        ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>(
                $"{this.GetType().Name} - {nameof(SetViewAutoRefreshDisabled)}");
        try
        {

            var response = await _mediator?.Send(new SetViewDisabledCommand(request.ViewId, request.OwnerId))!;

            _telemetryClient?.TrackEvent(nameof(SetViewAutoRefreshDisabled), new Dictionary<string, string>()
            {
                {"OwnerId", request.OwnerId},
                {"ViewId", request.ViewId},
                {"Response", ConvertToJsonObject(response)}
            });
            return new BoolResponse() {Result = response};
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
    /// Un-Freezes updates from data sets on a View
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<BoolResponse> SetViewAutoRefreshEnabled(SetViewAutoRefreshEnabledRequest request,
        ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>(
                $"{this.GetType().Name} - {nameof(SetViewAutoRefreshEnabled)}");
        try
        {
            var response = await _mediator?.Send(new SetViewEnabledCommand(request.ViewId, request.OwnerId))!;

            _telemetryClient?.TrackEvent(nameof(SetViewAutoRefreshEnabled), new Dictionary<string, string>()
            {
                {"OwnerId", request.OwnerId},
                {"ViewId", request.ViewId},
                {"Response", ConvertToJsonObject(response)}
            });

            return new BoolResponse() {Result = response};
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
    /// Sets the View Parameters on a owner view instance 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<BoolResponse> SetViewParameters(SetViewParametersRequest request,
        ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>(
                $"{this.GetType().Name} - {nameof(SetViewParameters)}");

        try
        {
            Dictionary<string, object> viewParameters = null;
            if ((request?.JsonPayload?.Trim() ?? "") != "")
                viewParameters = JsonSerializer.Deserialize<Dictionary<string, object>>(request.JsonPayload);

            var response =
                await _mediator?.Send(new SetViewParametersCommand(request.ViewId, request.OwnerId, viewParameters))!;

            _telemetryClient?.TrackEvent(nameof(SetViewParameters), new Dictionary<string, string>()
            {
                {"OwnerId", request.OwnerId},
                {"ViewId", request.ViewId},
                {"Response", ConvertToJsonObject(response)}
            });

            return new BoolResponse() {Result = response};
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
    /// Gets a list of the owners currently active views
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<MyViewResponse> GetMyViews(GetMyViewsRequest request, ServerCallContext context)
    {
        using var operation =
            _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(GetMyViews)}");
        try
        {

            var response =
                await _mediator?.Send(new GetMyViewsQuery(request.OwnerId))!;

            _telemetryClient?.TrackEvent(nameof(GetMyViews), new Dictionary<string, string>()
            {
                {"OwnerId", request.OwnerId},
                {"Response", ConvertToJsonObject(response)}
            });

            return new MyViewResponse()
            {
                Items =
                {
                    response.Select(x => new MyViewResponse.Types.MyView()
                    {
                        ViewId = x.ViewId,
                        ViewTypeName = x.ViewTypeName
                    })
                }
            };
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
        return convertObject == null ? "null" : JsonSerializer.Serialize(convertObject);
    }

    #endregion

}