using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace STAIExtensions.Host.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViewController : ControllerBase
{

    #region ctor
    public ViewController()
    {
        
    }
    #endregion
    
    
    [HttpGet(Name = "GetView")]
    #if !DEBUG
    [Authorize]
    #endif
    public Abstractions.Views.IDataSetView GetView(string viewId)
    {
        return null;
    }

    [HttpPost]
    [Route("CreateView")]
    public JsonResult CreateView(string viewType)
    {
        return null;
    }
    
    [HttpPost]
    [Route("AttachViewToDataset")]
    public JsonResult AttachViewToDataset(string viewType)
    {
        return null;
    }
    
    [HttpPost]
    [Route("DetachViewFromDataset")]
    public JsonResult DetachViewFromDataset(string viewType)
    {
        return null;
    }


    
    
}