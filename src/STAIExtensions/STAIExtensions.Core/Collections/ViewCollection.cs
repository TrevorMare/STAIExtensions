using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

public class ViewCollection : Abstractions.Collections.IViewCollection
{
    public IDataSetView? GetView(string id, string userSessionId)
    {
        throw new NotImplementedException();
    }

    public IDataSetView CreateView(string viewType, string ownerId)
    {
        throw new NotImplementedException();
    }
}