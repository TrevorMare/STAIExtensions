using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Host.SignalR.Hubs;

public interface ISTAIExtensionsHubClient
{

    /// <summary>
    /// Called from outside the scope of the hub
    /// </summary>
    /// <param name="dataSetId"></param>
    /// <returns></returns>
    Task OnDataSetUpdated(string dataSetId);
    
    /// <summary>
    /// Called from outside the scope of the hub
    /// </summary>
    /// <param name="viewId"></param>
    /// <returns></returns>
    Task OnDataSetViewUpdated(string viewId);

    /// <summary>
    /// Callback for when the data set view is created
    /// </summary>
    /// <param name="view"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnDataSetViewCreated(IDataSetView? view, string callbackId);

    /// <summary>
    /// Callback for when the data set view is created
    /// </summary>
    /// <param name="view"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnGetViewResponse(IDataSetView? view, string callbackId);

    /// <summary>
    /// Callback for the list data sets response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnListDataSetsResponse(IEnumerable<DataSetInformation> response, string callbackId);

    /// <summary>
    /// Callback for the Get Registered views response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnGetRegisteredViewsResponse(IEnumerable<ViewInformation> response, string callbackId);

    /// <summary>
    /// Callback for the remove view response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnRemoveViewResponse(bool response, string callbackId);
    
    /// <summary>
    /// Callback for the On Remove View Response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnAttachViewToDatasetResponse(bool response, string callbackId);
    
    /// <summary>
    /// Callback to remove the view from the data set
    /// </summary>
    /// <param name="response"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnDetachViewFromDatasetResponse(bool response, string callbackId);
    
    /// <summary>
    /// Callback to set the view parameters
    /// </summary>
    /// <param name="response"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnSetViewParametersResponse(bool response, string callbackId);
    
    /// <summary>
    /// Callback for thr set auto refresh view enabled
    /// </summary>
    /// <param name="response"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnSetViewAutoRefreshEnabledResponse(bool response, string callbackId);
    
    /// <summary>
    /// Callback for thr set auto refresh view enabled
    /// </summary>
    /// <param name="response"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnSetViewAutoRefreshDisabledResponse(bool response, string callbackId);
    
    /// <summary>
    /// Callback for the get my views 
    /// </summary>
    /// <param name="response"></param>
    /// <param name="callbackId"></param>
    /// <returns></returns>
    Task OnGetMyViewsResponse(IEnumerable<MyViewInformation> response, string callbackId);
}