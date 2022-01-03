using System.Reflection;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.CQRS.DataSets.Queries;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;

namespace STAIExtensions.Host.Grpc.Services;

public class STAIExtensionsGrpcService : STAIExtensions.Host.Grpc.STAIExtensionsGrpcService.STAIExtensionsGrpcServiceBase
{
    
    #region Members

    private readonly ILogger<STAIExtensionsGrpcService> _logger;
    private readonly IMediator? _mediator;

    #endregion

    #region ctor
    public STAIExtensionsGrpcService(ILogger<STAIExtensionsGrpcService> logger)
    {
        _mediator = Abstractions.DependencyExtensions.Mediator;
        _logger = logger;
    }
    #endregion

    #region Methods
    public override async Task<IDataSetView> CreateView(CreateViewRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new CreateViewCommand(request.ViewType, request.OwnerId))!;
        
        var result = new IDataSetView()
        {
            ExpiryDate = response.ExpiryDate.HasValue ? Timestamp.FromDateTime(response.ExpiryDate.Value) : null,
            Id = response.Id,
            LastUpdate = response.LastUpdate.HasValue ? Timestamp.FromDateTime(response.LastUpdate.Value) : null,
            OwnerId = response.OwnerId,
            RefreshEnabled = response.RefreshEnabled,
            SlidingExpiration = Duration.FromTimeSpan(response.SlidingExpiration)
        };

        if (response?.ViewParameterDescriptors != null)
        {
            var parameterDescriptors = response?.ViewParameterDescriptors?.Select(x => new DataSetViewParameterDescriptor()
            {
                Description = x.Description,
                Name = x.Name,
                Required = x.Required,
                Type = x.Type
            });
            
            result.ViewParameterDescriptors.AddRange(parameterDescriptors);
        }

        return result;
    }

    public override async Task<IDataSetView> GetView(GetViewRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new GetDataViewQuery(request.ViewId, request.OwnerId))!;
        if (response == null) return new IDataSetView();
        
        var result = new IDataSetView()
        {
            ExpiryDate = response.ExpiryDate.HasValue ? Timestamp.FromDateTime(response.ExpiryDate.Value) : null,
            Id = response.Id,
            LastUpdate = response.LastUpdate.HasValue ? Timestamp.FromDateTime(response.LastUpdate.Value) : null,
            OwnerId = response.OwnerId,
            RefreshEnabled = response.RefreshEnabled,
            SlidingExpiration = Duration.FromTimeSpan(response.SlidingExpiration)
        };

        if (response?.ViewParameterDescriptors != null)
        {
            var parameterDescriptors = response?.ViewParameterDescriptors?.Select(x => new DataSetViewParameterDescriptor()
            {
                Description = x.Description,
                Name = x.Name,
                Required = x.Required,
                Type = x.Type
            });
            
            result.ViewParameterDescriptors.AddRange(parameterDescriptors);
        }

        return result;
    }

    public override async Task<ListDataSetsResponse> ListDataSets(NoParameters request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new ListDataSetsQuery())!;

        return new ListDataSetsResponse()
        {
            Items =
            {
                response.Select(x => new DataSetInformation()
                {
                    DataSetId = x.DataSetId,
                    DataSetName = x.DataSetName,
                    DataSetType = x.DataSetType
                })
            }
        };
    }

    public override async Task<GetRegisteredViewsResponse> GetRegisteredViews(NoParameters request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new GetRegisteredViewsQuery())!;

        return new GetRegisteredViewsResponse()
        {
            Items =
            {
                response.Select(x => new DataSetViewInformation()
                {
                   ViewName = x.ViewName,
                   ViewTypeName = x.ViewTypeName,
                   DataSetViewParameterDescriptors =
                   {
                       x?.DataSetViewParameterDescriptors?.Select(param => new DataSetViewParameterDescriptor()
                       {
                           Description = param.Description,
                           Name = param.Name,
                           Required = param.Required,
                           Type = param.Type
                       })
                   }
                })
            }
        };
    }

    public override async Task<BoolResponseMessage> RemoveView(RemoveViewRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new RemoveViewCommand( request.ViewId ))!;
        return new BoolResponseMessage() {Result = response};
    }

    public override async Task<BoolResponseMessage> AttachViewToDataset(AttachViewToDatasetRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new AttachViewToDataSetCommand( request.ViewId, request.DataSetId, request.OwnerId ))!;
        return new BoolResponseMessage() {Result = response};
    }

    public override async Task<BoolResponseMessage> DetachViewFromDataset(DetachViewFromDatasetRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new DetachViewFromDataSetCommand( request.ViewId, request.DataSetId, request.OwnerId ))!;
        return new BoolResponseMessage() {Result = response};
    }

    // public override async Task<BoolResponseMessage> SetViewParameters(SetViewParametersRequest request, ServerCallContext context)
    // {
    //     var response = await _mediator?.Send(new DetachViewFromDataSetCommand( request.ViewId, request.da, request.OwnerId ))!;
    //     return new BoolResponseMessage() {Result = response};
    // }

    public override async Task<BoolResponseMessage> SetViewAutoRefreshDisabled(SetViewAutoRefreshEnabledRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new SetViewDisabledCommand( request.ViewId, request.OwnerId ))!;
        return new BoolResponseMessage() {Result = response};
    }

    public override async Task<BoolResponseMessage> SetViewAutoRefreshEnabled(SetViewAutoRefreshEnabledRequest request, ServerCallContext context)
    {
        var response = await _mediator?.Send(new SetViewEnabledCommand( request.ViewId, request.OwnerId ))!;
        return new BoolResponseMessage() {Result = response};
    }

    #endregion
   
    
}