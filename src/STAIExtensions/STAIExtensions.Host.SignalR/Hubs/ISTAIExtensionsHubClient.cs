using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Host.SignalR.Hubs;


/// <summary>
/// Interface for the SignalR Hub Push messages
/// </summary>
public interface ISTAIExtensionsHubClient
{

    /// <summary>
    /// Called from outside the scope of the hub. This is called to notify users that a Data Set has been updated
    /// </summary>
    /// <param name="dataSetId">The Data Set Id that updated</param>
    /// <returns></returns>
    Task OnDataSetUpdated(string dataSetId);
    
    /// <summary>
    /// Called from outside the scope of the hub. This is called to notify users that a Data View has been updated
    /// </summary>
    /// <param name="viewId">The view Id that updated</param>
    /// <returns></returns>
    Task OnDataSetViewUpdated(string viewId);

    /// <summary>
    /// Push response of the Data Set View Created result
    /// </summary>
    /// <param name="view">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnDataSetViewCreated(IDataSetView? view, string callbackId);

    /// <summary>
    /// Push response of the Get View result
    /// </summary>
    /// <param name="view">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnGetViewResponse(IDataSetView? view, string callbackId);

    /// <summary>
    /// Push response of the List Data Sets result
    /// </summary>
    /// <param name="response">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnListDataSetsResponse(IEnumerable<DataSetInformation> response, string callbackId);

    /// <summary>
    /// Push response of the Get Registered views result
    /// </summary>
    /// <param name="response">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnGetRegisteredViewsResponse(IEnumerable<ViewInformation> response, string callbackId);

    /// <summary>
    /// Push response of the Remove View Created result
    /// </summary>
    /// <param name="response">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnRemoveViewResponse(bool response, string callbackId);
    
    /// <summary>
    /// Push response of the Attach View To Data Set result
    /// </summary>
    /// <param name="response">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnAttachViewToDatasetResponse(bool response, string callbackId);
    
    /// <summary>
    /// Push response of the Detach View from Data Set result
    /// </summary>
    /// <param name="response">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnDetachViewFromDatasetResponse(bool response, string callbackId);
    
    /// <summary>
    /// Push response of the Set View Parameters result
    /// </summary>
    /// <param name="response">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnSetViewParametersResponse(bool response, string callbackId);
    
    /// <summary>
    /// Push response of the Set View Auto Refresh Enabled result
    /// </summary>
    /// <param name="response">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnSetViewAutoRefreshEnabledResponse(bool response, string callbackId);
    
    /// <summary>
    /// Push response of the Set View Auto Refresh Disabled result
    /// </summary>
    /// <param name="response">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnSetViewAutoRefreshDisabledResponse(bool response, string callbackId);
    
    /// <summary>
    /// Push response of the Get My Views result
    /// </summary>
    /// <param name="response">The result of the previous action</param>
    /// <param name="callbackId">An Id that was passed in the initial call</param>
    /// <returns></returns>
    Task OnGetMyViewsResponse(IEnumerable<MyViewInformation> response, string callbackId);
}