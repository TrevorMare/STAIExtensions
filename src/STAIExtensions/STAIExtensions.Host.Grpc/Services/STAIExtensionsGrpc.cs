using System.Reflection;
using System.Text.Json;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.CQRS.DataSets.Queries;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;
using STAIExtensions.Host.Grpc.Mapping;

namespace STAIExtensions.Host.Grpc.Services;

public class STAIExtensionsGrpcService : STAIExtensions.Host.Grpc.STAIExtensionsGrpcService.STAIExtensionsGrpcServiceBase
{
    
    #region Members

    private readonly ILogger<STAIExtensionsGrpcService> _logger;
    private readonly IMediator? _mediator;
    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;

    private readonly Queue<string> _viewUpdates = new();
    private readonly Queue<string> _datasetUpdates = new();
    #endregion

    #region ctor
    public STAIExtensionsGrpcService(ILogger<STAIExtensionsGrpcService> logger)
    {
        _mediator = Abstractions.DependencyExtensions.Mediator;
        _logger = logger;
        _viewCollection = Abstractions.DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>()!;
        _dataSetCollection = Abstractions.DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>()!;
    }
    #endregion

    #region Methods

    public override async Task OnDataSetUpdated(NoParameters request, IServerStreamWriter<OnDataSetUpdatedResponse> responseStream, ServerCallContext context)
    {
        _dataSetCollection.OnDataSetUpdated += (sender, args) =>
        {
            this._datasetUpdates.Enqueue(sender.DataSetId);
        };

        var contextCancellationToken = context.CancellationToken;
        
        while (true && contextCancellationToken.IsCancellationRequested == false)
        {
            while (this._datasetUpdates.Count > 0)
            {
                string dataSetId = this._datasetUpdates.Dequeue();
                await responseStream.WriteAsync(new OnDataSetUpdatedResponse() {DataSetId = dataSetId});
            }
            await Task.Delay(200, contextCancellationToken);
        }
    }

    public override async Task OnDataViewUpdated(OnDataSetViewUpdatedRequest request, IServerStreamWriter<OnDataSetViewUpdatedResponse> responseStream, ServerCallContext context)
    {
        _viewCollection.OnDataSetViewUpdated += (sender, args) =>
        {
            if (string.Equals(sender?.OwnerId?.Trim(), request.OwnerId.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                this._viewUpdates.Enqueue(sender.Id);    
            }
        };

        var contextCancellationToken = context.CancellationToken;
        
        while (true && contextCancellationToken.IsCancellationRequested == false)
        {
            while (this._viewUpdates.Count > 0)
            {
                string viewId = this._viewUpdates.Dequeue();
                await responseStream.WriteAsync(new OnDataSetViewUpdatedResponse() { ViewId = viewId });
            }
            await Task.Delay(200, contextCancellationToken);
        }
    }

    public override async Task<IDataSetView> CreateView(CreateViewRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new CreateViewCommand(request.ViewType, request.OwnerId))!;
        return ModelMapper.ConvertViewToDataSetViewResponse(response);
    }
    
    public override async Task<IDataSetViewJson> GetViewJson(GetViewRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new GetDataViewQuery(request.ViewId, request.OwnerId))!;
        if (response == null) return new IDataSetViewJson();

        return new IDataSetViewJson()
        {
            Payload = ModelMapper.ConvertViewToJson(response)
        };
    }

    public override async Task<IDataSetView> GetView(GetViewRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new GetDataViewQuery(request.ViewId, request.OwnerId))!;
        return ModelMapper.ConvertViewToDataSetViewResponse(response);
    }

    public override async Task<ListDataSetsResponse> ListDataSets(NoParameters request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new ListDataSetsQuery())!;
        return ModelMapper.ConvertDataSetInformation(response);
    }

    public override async Task<GetRegisteredViewsResponse> GetRegisteredViews(NoParameters request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new GetRegisteredViewsQuery())!;
        return ModelMapper.ConvertViewInformationRegisteredViewsResponse(response);
    }

    public override async Task<BoolResponse> RemoveView(RemoveViewRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new RemoveViewCommand( request.ViewId ))!;
        return new BoolResponse() { Result = response };
    }

    public override async Task<BoolResponse> AttachViewToDataset(AttachViewToDatasetRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new AttachViewToDataSetCommand( request.ViewId, request.DataSetId, request.OwnerId ))!;
        return new BoolResponse() { Result = response };
    }

    public override async Task<BoolResponse> DetachViewFromDataset(DetachViewFromDatasetRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new DetachViewFromDataSetCommand( request.ViewId, request.DataSetId, request.OwnerId ))!;
        return new BoolResponse() { Result = response };
    }

    public override async Task<BoolResponse> SetViewAutoRefreshDisabled(SetViewAutoRefreshDisabledRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new SetViewDisabledCommand( request.ViewId, request.OwnerId ))!;
        return new BoolResponse() { Result = response };
    }

    public override async Task<BoolResponse> SetViewAutoRefreshEnabled(SetViewAutoRefreshEnabledRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new SetViewEnabledCommand( request.ViewId, request.OwnerId ))!;
        return new BoolResponse() {Result = response};
    }

    public override async Task<BoolResponse> SetViewParameters(SetViewParametersRequest request, ServerCallContext context)
    {
        Dictionary<string, object> viewParameters = null;
        if ((request?.JsonPayload?.Trim() ?? "") != "")
            viewParameters = JsonSerializer.Deserialize<Dictionary<string, object>>(request.JsonPayload);

        var response =
            await _mediator?.Send(new SetViewParametersCommand(request.ViewId, request.OwnerId, viewParameters))!;
        
        return new BoolResponse() { Result = response};
    }

    public override async Task<MyViewResponse> GetMyViews(GetMyViewsRequest request, ServerCallContext context)
    {
        var response =
            await _mediator?.Send(new GetMyViewsQuery(request.OwnerId))!;

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

    #endregion
   
    
}