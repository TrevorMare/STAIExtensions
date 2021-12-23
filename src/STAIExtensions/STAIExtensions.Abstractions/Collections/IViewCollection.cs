using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Collections;

public interface IViewCollection
{

    IDataSetView? GetView(string id, string userSessionId);

    IDataSetView CreateView(string viewType, string ownerId);

}