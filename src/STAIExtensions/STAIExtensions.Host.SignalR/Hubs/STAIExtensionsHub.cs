using MediatR;
using Microsoft.AspNetCore.SignalR;
using STAIExtensions.Abstractions.CQRS.DataSets.Queries;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;
using STAIExtensions.Host.SignalR.Identity;

namespace STAIExtensions.Host.SignalR.Hubs;

// TODO: [Authorize]
internal class STAIExtensionsHub : Hub<ISTAIExtensionsHubClient>
{

    #region Members
    private readonly IMediator? _mediator;
    private readonly ISignalRUserGroups _signalRUserGroups;
    #endregion

    #region ctor
    public STAIExtensionsHub(ISignalRUserGroups signalRUserGroups)
    {
        _mediator = Abstractions.DependencyExtensions.Mediator;
        _signalRUserGroups = signalRUserGroups ?? throw new ArgumentNullException(nameof(signalRUserGroups));
    }
    #endregion

    #region Public Methods

    public async Task CreateView(string viewType, string ownerId, string callBackId)
    {
        var response = await _mediator?.Send(new CreateViewCommand(viewType, ownerId))!;

        string groupName = $"{ownerId}_{response.Id}";

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _signalRUserGroups.RegisterUserGroupView(response, groupName);

        await Clients.Caller.OnDataSetViewCreated(response, callBackId);
    }

    public async Task GetView(string viewId, string ownerId, string callBackId)
    {
        var response = await _mediator?.Send(new GetDataViewQuery(viewId, ownerId))!;
        await Clients.Caller.OnGetViewResponse(response, callBackId);
    }
    
    public async Task ListDataSets(string callBackId)
    {
        var response = await _mediator?.Send(new ListDataSetsQuery())!;
        await Clients.Caller.OnListDataSetsResponse(response, callBackId);
    }
    
    public async Task GetRegisteredViews(string callBackId)
    {
        var response = await _mediator?.Send(new GetRegisteredViewsQuery())!;
        await Clients.Caller.OnGetRegisteredViewsResponse(response, callBackId);
    }
    
    public async Task RemoveView(string viewId, string callBackId)
    {
        var response = await _mediator?.Send(new RemoveViewCommand(viewId))!;
        
        _signalRUserGroups.DeRegisterUserGroupView(viewId);
        
        await Clients.Caller.OnRemoveViewResponse(response, callBackId);
    }

    public async Task AttachViewToDataset(string viewId, string dataSetId, string ownerId, string callBackId)
    {
        var response = await _mediator?.Send(new AttachViewToDataSetCommand(viewId, dataSetId, ownerId))!;
        await Clients.Caller.OnAttachViewToDatasetResponse(response, callBackId);
    }
    
    public async Task DetachViewFromDataset(string viewId, string dataSetId, string ownerId, string callBackId)
    {
        var response = await _mediator?.Send(new DetachViewFromDataSetCommand(viewId, dataSetId, ownerId))!;
        await Clients.Caller.OnDetachViewFromDatasetResponse(response, callBackId);
    }
    
    public async Task SetViewParameters(string viewId, string ownerId, Dictionary<string, object> viewParameters, string callBackId)
    {
        var response = await _mediator?.Send(new SetViewParametersCommand(viewId, ownerId, viewParameters))!;
        await Clients.Caller.OnSetViewParametersResponse(response, callBackId);
    }
    #endregion
    
}