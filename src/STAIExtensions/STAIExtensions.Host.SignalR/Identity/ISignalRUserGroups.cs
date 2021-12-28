using Microsoft.AspNetCore.SignalR;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Host.SignalR.Identity;

public interface ISignalRUserGroups
{

    IEnumerable<string> FindGroupNames(string viewId);

    void RegisterUserGroupView(IDataSetView response, string groupName);
    
    void DeRegisterUserGroupView(string viewId);
}