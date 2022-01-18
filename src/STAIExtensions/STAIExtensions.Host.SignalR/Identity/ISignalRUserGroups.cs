using Microsoft.AspNetCore.SignalR;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Host.SignalR.Identity;

public interface ISignalRUserGroups
{

    /// <summary>
    ///  Gets all the group names for the view
    /// </summary>
    /// <param name="viewId"></param>
    /// <returns></returns>
    IEnumerable<string> FindGroupNames(string viewId);

    /// <summary>
    /// Registers a new group name for a view
    /// </summary>
    /// <param name="response"></param>
    /// <param name="groupName"></param>
    void RegisterUserGroupView(IDataSetView response, string groupName);
    
    /// <summary>
    /// De-Registers a view from all groups
    /// </summary>
    /// <param name="viewId"></param>
    void DeRegisterUserGroupView(string viewId);
}