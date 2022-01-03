using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;
using STAIExtensions.Host.Api.Models;
using STAIExtensions.Host.Api.Security;

namespace STAIExtensions.Host.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiKey()]
public class ViewController : ControllerBase
{

    #region Members

    private readonly IMediator _mediator;

    #endregion

    #region ctor
    public ViewController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    #endregion

    #region Methods
    [HttpGet]
    [Route("GetView")]
    public async Task<Abstractions.Views.IDataSetView?> GetView(string viewId, string ownerId)
    {
        return await _mediator.Send(new GetDataViewQuery(viewId, ownerId));
    }
    
    [HttpGet]
    [Route("GetRegisteredViews")]
    public async Task<IEnumerable<ViewInformation>> GetRegisteredViews()
    {
        return await _mediator.Send(new GetRegisteredViewsQuery());
    }

    [HttpPost]
    [Route("CreateView")]
    public async Task<Abstractions.Views.IDataSetView> CreateView([FromBody]CreateViewRequest request)
    {
        return await _mediator.Send(new CreateViewCommand(request.ViewType, request.OwnerId));
    }
    
    [HttpPost]
    [Route("RemoveView")]
    public async Task<bool> RemoveView([FromBody]RemoveViewRequest request)
    {
        return await _mediator.Send(new RemoveViewCommand(request.ViewId));
    }
    
    [HttpPost]
    [Route("AttachViewToDataset")]
    public async Task<bool> AttachViewToDataset([FromBody]AttachViewToDatasetRequest request)
    {
        return await _mediator.Send(new AttachViewToDataSetCommand(request.ViewId, request.DataSetId, request.OwnerId));
    }
    
    [HttpPost]
    [Route("DetachViewFromDataset")]
    public async Task<bool> DetachViewFromDataset(DetachViewFromDatasetRequest request)
    {
        return await _mediator.Send(new DetachViewFromDataSetCommand(request.ViewId, request.DataSetId, request.OwnerId));
    }
    
    [HttpPost]
    [Route("SetViewParameters")]
    public async Task<bool> SetViewParameters(SetViewParametersRequest request)
    { 
        return await _mediator.Send(new SetViewParametersCommand(request.ViewId, request.OwnerId, request.ViewParameters));
    }
    
    [HttpPost]
    [Route("SetViewAutoRefreshEnabled")]
    public async Task<bool> SetViewAutoRefreshEnabled(SetViewAutoRefreshEnabledRequest request)
    { 
        return await _mediator.Send(new SetViewEnabledCommand(request.ViewId, request.OwnerId));
    }
    
    [HttpPost]
    [Route("SetViewAutoRefreshDisabled")]
    public async Task<bool> SetViewAutoRefreshDisabled(SetViewAutoRefreshDisabledRequest request)
    { 
        return await _mediator.Send(new SetViewDisabledCommand(request.ViewId, request.OwnerId));
    }
    #endregion
    
}