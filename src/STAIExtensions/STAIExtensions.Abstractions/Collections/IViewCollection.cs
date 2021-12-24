using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Collections;

public interface IViewCollection
{
    
    bool UseStrictUserSession { get; set; }

    IDataSetView? GetView(string id, string userSessionId);

    IDataSetView CreateView(string viewTypeName, string ownerId);

    IDataSetView? GetViewForUpdate(string id);
}