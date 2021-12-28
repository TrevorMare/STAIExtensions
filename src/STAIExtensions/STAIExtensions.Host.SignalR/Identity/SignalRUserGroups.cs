using Microsoft.AspNetCore.SignalR;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Host.SignalR.Identity;

public class SignalRUserGroups : ISignalRUserGroups
{

    #region Members

    private readonly Dictionary<string, List<string>> _groupNameViewMapping = new(StringComparer.OrdinalIgnoreCase);

    #endregion
    
    public IEnumerable<string> FindGroupNames(string viewId)
    {
        return _groupNameViewMapping.ContainsKey(viewId) ? _groupNameViewMapping[viewId] : new List<string>();
    }

    public void RegisterUserGroupView(IDataSetView response, string groupName)
    {
        var viewId = response.Id;

        if (!_groupNameViewMapping.ContainsKey(viewId))
            _groupNameViewMapping[viewId] = new List<string>();
        
        _groupNameViewMapping[viewId].Add(groupName);
    }

    public void DeRegisterUserGroupView(string viewId)
    {
        if (_groupNameViewMapping.ContainsKey(viewId))
            _groupNameViewMapping.Remove(viewId);
    }
}