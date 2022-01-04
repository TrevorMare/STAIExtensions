using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Collections;

public interface IViewCollection
{

    delegate void OnDataSetViewUpdatedHandler(IDataSetView sender, EventArgs e);

    event OnDataSetViewUpdatedHandler OnDataSetViewUpdated;
    
    bool ViewsExpire { get; }
    
    bool UseStrictViews { get; }

    int? MaximumViews { get; }
    
    int ViewCount { get; }
    
    TimeSpan? DefaultSlidingExpiryTimeSpan { get; }

    IDataSetView? GetView(string id, string? ownerId);

    IDataSetView CreateView(string viewTypeName, string? ownerId);

    IDataSetView? GetViewForUpdate(string id);

    IEnumerable<IDataSetView> GetExpiredViews();
    
    void RemoveView(IDataSetView expiredView);
    
    void RemoveView(string viewId);

    void SetViewParameters(string viewId, string? ownerId, Dictionary<string, object>? requestViewParameters);
    
    IEnumerable<MyViewInformation> GetMyViews(string ownerId);
}