using MediatR;
using Microsoft.AspNetCore.Mvc;
using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Host.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpGet]
    [Route("ListDataSets")]
    public async Task<IEnumerable<DataSetInformation>> ListDataSets()
    {
        return await _mediator.Send(new Abstractions.CQRS.Queries.ListDataSetsQuery());
    }
    #endregion

}