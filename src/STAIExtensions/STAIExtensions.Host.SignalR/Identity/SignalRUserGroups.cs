using Microsoft.AspNetCore.SignalR;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Host.SignalR.Identity;

/// <summary>
/// Class to keep track of connection groups and owners of views
/// </summary>
public class SignalRUserGroups : ISignalRUserGroups
{

    #region Members

    private readonly Dictionary<string, List<string>> _groupNameViewMapping = new(StringComparer.OrdinalIgnoreCase);

    #endregion
    /// <summary>
    /// Gets all the group names for the view
    /// </summary>
    /// <param name="viewId">The view Id of the groups</param>
    /// <returns></returns>
    public IEnumerable<string> FindGroupNames(string viewId)
    {
        return _groupNameViewMapping.ContainsKey(viewId) ? _groupNameViewMapping[viewId] : new List<string>();
    }

    /// <summary>
    /// Registers a new group name for a view
    /// </summary>
    /// <param name="response"></param>
    /// <param name="groupName"></param>
    public void RegisterUserGroupView(IDataSetView response, string groupName)
    {
        var viewId = response.Id;

        if (!_groupNameViewMapping.ContainsKey(viewId))
            _groupNameViewMapping[viewId] = new List<string>();
        
        _groupNameViewMapping[viewId].Add(groupName);
    }

    /// <summary>
    /// De-Registers a view from all groups
    /// </summary>
    /// <param name="viewId">The view Id of the groups</param>
    public void DeRegisterUserGroupView(string viewId)
    {
        if (_groupNameViewMapping.ContainsKey(viewId))
            _groupNameViewMapping.Remove(viewId);
    }
}