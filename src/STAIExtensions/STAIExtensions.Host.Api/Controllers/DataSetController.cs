using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.CQRS.DataSets.Queries;
using STAIExtensions.Host.Api.Security;

namespace STAIExtensions.Host.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiKey()]
public class DataSetController : ControllerBase
{

    #region Members

    private readonly IMediator _mediator;

    #endregion

    #region ctor
    public DataSetController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    #endregion

    #region Methods
    /// <summary>
    /// Lists the Data Sets currently registered in the system
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/DataSet/ListDataSets
    ///   
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<DataSetInformation>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<DataSetInformation>> ListDataSets()
    {
        return await _mediator.Send(new ListDataSetsQuery());
    }
    #endregion

}