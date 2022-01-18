using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    /// <summary>
    /// Gets a view with the Id an owner Id
    /// </summary>
    /// <param name="viewId">The View Id to find</param>
    /// <param name="ownerId">The Owner Id of the view</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/View/GetView?viewId=123&ownerId=abc
    ///   
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Abstractions.Views.IDataSetView), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Abstractions.Views.IDataSetView?> GetView(string viewId, string ownerId)
    {
        return await _mediator.Send(new GetDataViewQuery(viewId, ownerId));
    }
    
    /// <summary>
    /// Gets a list of registered view types that can be created 
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/View/GetRegisteredViews
    ///   
    /// </remarks>  
    /// <returns></returns>
    [HttpGet]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ViewInformation>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<ViewInformation>> GetRegisteredViews()
    {
        return await _mediator.Send(new GetRegisteredViewsQuery());
    }

    /// <summary>
    /// Creates a view with the specified type for the Owner specified
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/View/CreateView
    ///     {
    ///        "viewType": "FullyQualifiedTypeName",
    ///        "ownerId": "Abc"
    ///     } 
    ///   
    /// </remarks>  
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Abstractions.Views.IDataSetView), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Abstractions.Views.IDataSetView> CreateView([FromBody]CreateViewRequest request)
    {
        return await _mediator.Send(new CreateViewCommand(request.ViewType, request.OwnerId));
    }
    
    /// <summary>
    /// Removes a view from the collection manager and all links to Data Sets
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/View/RemoveView
    ///     {
    ///        "viewId": "1239821"
    ///     } 
    ///   
    /// </remarks>  
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<bool> RemoveView([FromBody]RemoveViewRequest request)
    {
        return await _mediator.Send(new RemoveViewCommand(request.ViewId));
    }
    
    /// <summary>
    /// Links a view to a data set for updates
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/View/AttachViewToDataset
    ///     {
    ///        "viewId": "1239821",
    ///        "dataSetId": "321",
    ///        "ownerId": "Abc"
    ///     } 
    ///   
    /// </remarks>  
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<bool> AttachViewToDataset([FromBody]AttachViewToDatasetRequest request)
    {
        return await _mediator.Send(new AttachViewToDataSetCommand(request.ViewId, request.DataSetId, request.OwnerId));
    }
    
    /// <summary>
    /// De-Links a view from a data set for updates
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/View/DetachViewFromDataset
    ///     {
    ///        "viewId": "1239821",
    ///        "dataSetId": "321",
    ///        "ownerId": "Abc"
    ///     } 
    ///   
    /// </remarks>  
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<bool> DetachViewFromDataset(DetachViewFromDatasetRequest request)
    {
        return await _mediator.Send(new DetachViewFromDataSetCommand(request.ViewId, request.DataSetId, request.OwnerId));
    }
    
    /// <summary>
    /// Sets the parameters on a view
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/View/SetViewParameters
    ///     {
    ///        "viewId": "1239821",
    ///        "viewParameters": {
    ///           "param1": 123
    ///        },
    ///        "ownerId": "Abc"
    ///     } 
    ///   
    /// </remarks>  
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<bool> SetViewParameters(SetViewParametersRequest request)
    { 
        return await _mediator.Send(new SetViewParametersCommand(request.ViewId, request.OwnerId, request.ViewParameters));
    }
    
    /// <summary>
    /// Freezes a view temporarily for updates
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/View/SetViewAutoRefreshEnabled
    ///     {
    ///        "viewId": "1239821",
    ///        "ownerId": "Abc"
    ///     } 
    ///   
    /// </remarks>  
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<bool> SetViewAutoRefreshEnabled(SetViewAutoRefreshEnabledRequest request)
    { 
        return await _mediator.Send(new SetViewEnabledCommand(request.ViewId, request.OwnerId));
    }
    
    /// <summary>
    /// Un-Freezes a view temporarily for updates
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/View/SetViewAutoRefreshDisabled
    ///     {
    ///        "viewId": "1239821",
    ///        "ownerId": "Abc"
    ///     } 
    ///   
    /// </remarks>  
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<bool> SetViewAutoRefreshDisabled(SetViewAutoRefreshDisabledRequest request)
    { 
        return await _mediator.Send(new SetViewDisabledCommand(request.ViewId, request.OwnerId));
    }
    
    /// <summary>
    /// Loads a list of the owner active views
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/View/GetMyViews
    ///     {
    ///        "ownerId": "Abc"
    ///     } 
    ///   
    /// </remarks>  
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<MyViewInformation>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<MyViewInformation>> GetMyViews(GetMyViewsRequest request)
    { 
        return await _mediator.Send(new GetMyViewsQuery((request.OwnerId)));
    }
    #endregion
    
}