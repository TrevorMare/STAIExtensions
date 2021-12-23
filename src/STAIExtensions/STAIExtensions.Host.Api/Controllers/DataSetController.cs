using Microsoft.AspNetCore.Mvc;

namespace STAIExtensions.Host.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataSetController : ControllerBase
{

    [HttpGet]
    [Route("ListDataSets")]
    public IEnumerable<string> ListDataSets()
    {

        return null;

    }

}