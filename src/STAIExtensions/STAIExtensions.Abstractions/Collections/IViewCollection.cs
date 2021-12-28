using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Collections;

public interface IViewCollection
{

    delegate void OnDataSetViewUpdatedHandler(IDataSetView sender, EventArgs e);

    event OnDataSetViewUpdatedHandler OnDataSetViewUpdated;
    
    bool ViewsExpire { get; }
    
    IDataSetView? GetView(string id, string ownerId);

    IDataSetView CreateView(string viewTypeName, string ownerId);

    IDataSetView? GetViewForUpdate(string id);

    IEnumerable<IDataSetView> GetExpiredViews();
    
    void RemoveView(IDataSetView expiredView);
    
    void RemoveView(string viewId);
    
}