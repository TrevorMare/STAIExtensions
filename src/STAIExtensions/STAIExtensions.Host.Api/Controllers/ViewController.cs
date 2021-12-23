using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STAIExtensions.Abstractions.CQRS.Commands;
using STAIExtensions.Abstractions.CQRS.Queries;

namespace STAIExtensions.Host.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    
    
    [HttpGet(Name = "GetView")]
    #if !DEBUG
    [Authorize]
    #endif
    public async Task<Abstractions.Views.IDataSetView> GetView(string viewId)
    {
        return await _mediator.Send(new GetDataViewQuery(viewId, ""));
    }

    [HttpPost]
    [Route("CreateView")]
    public async Task<Abstractions.Views.IDataSetView> CreateView(string viewType)
    {
        return await _mediator.Send(new CreateViewCommand(viewType, ""));
    }
    
    [HttpPost]
    [Route("AttachViewToDataset")]
    public async Task<bool> AttachViewToDataset(string viewId, string dataSetId)
    {
        return await _mediator.Send(new AttachViewToDataSetCommand(viewId, dataSetId, ""));
    }
    
    [HttpPost]
    [Route("DetachViewFromDataset")]
    public async Task<bool> DetachViewFromDataset(string viewId, string dataSetId)
    {
        return await _mediator.Send(new DetachViewFromDataSetCommand(viewId, dataSetId, ""));
    }


    
    
}