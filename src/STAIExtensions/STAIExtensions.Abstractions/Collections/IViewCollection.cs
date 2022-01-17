using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Collections;

/// <summary>
/// Defines an interface for the View Collection.
/// </summary>
public interface IViewCollection
{

    /// <summary>
    /// Delegate for the event OnDataSetViewUpdated <see cref="OnDataSetViewUpdated"/>
    /// </summary>
    delegate void OnDataSetViewUpdatedHandler(IDataSetView sender, EventArgs e);

    /// <summary>
    /// Event that is fired when a DataSetView object is updated
    /// </summary>
    event OnDataSetViewUpdatedHandler OnDataSetViewUpdated;
    
    /// <summary>
    /// Gets a value from the options indicating if Views should expire after a period. The expiration of
    /// a view is reset each time the GetView <see cref="GetView"/> is called. 
    /// </summary>
    bool ViewsExpire { get; }
    
    /// <summary>
    /// Gets a value from the options indicating if the owner Id of a view can be blank or empty. This will allow
    /// for shared views between users but might have some complications when shared parameters are set.
    /// </summary>
    bool UseStrictViews { get; }

    /// <summary>
    /// Gets a value from the options indicating the total number of allowed views that can be created globally
    /// </summary>
    int? MaximumViews { get; }
    
    /// <summary>
    /// Gets the current number of active views in the collection
    /// </summary>
    int ViewCount { get; }
    
    /// <summary>
    /// Gets or sets the sliding expiration timespan on the View that will be added on each View Retrieval <see cref="GetView"/>
    /// The view is initialised with from the value passed in the options. 
    /// </summary>
    TimeSpan? DefaultSlidingExpiryTimeSpan { get; }

    /// <summary>
    /// Retrieves an active view in it's current state. This method will reset the expiration date and will have to be called
    /// within the time frame specified to keep the view alive.
    /// </summary>
    /// <param name="id">The unique Id of the View</param>
    /// <param name="ownerId">The optional owner identifier that created the view</param>
    /// <returns></returns>
    IDataSetView? GetView(string id, string? ownerId);

    /// <summary>
    /// Creates a new user instance view of the specified type 
    /// </summary>
    /// <param name="viewTypeName">The type of of the view to create an instance of</param>
    /// <param name="ownerId">The optional user/owner Id of this view</param>
    /// <returns></returns>
    IDataSetView CreateView(string viewTypeName, string? ownerId);
    
    /// <summary>
    /// Internal method that will retrieve a view without the Owner id specified. This
    /// should only be used to retrieve a view from the collection for update logic 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IDataSetView? GetViewForUpdate(string id);

    /// <summary>
    /// Gets a list of views that has expired and needs to be removed from the collection
    /// </summary>
    /// <returns></returns>
    IEnumerable<IDataSetView> GetExpiredViews();
    
    /// <summary>
    /// Removes a view from the collection
    /// </summary>
    /// <param name="expiredView">The view to remove</param>
    void RemoveView(IDataSetView expiredView);
    
    /// <summary>
    /// Removes a view from the collection
    /// </summary>
    /// <param name="viewId">The unique view Id to remove</param>
    void RemoveView(string viewId);

    /// <summary>
    /// Sets the View Parameters.
    /// </summary>
    /// <param name="viewId">The unique View Id to set the parameters on</param>
    /// <param name="ownerId">The owner of the view</param>
    /// <param name="requestViewParameters">The parameter values to set on the view</param>
    void SetViewParameters(string viewId, string? ownerId, Dictionary<string, object>? requestViewParameters);
    
    /// <summary>
    /// Gets a list of views from the internal collection by Owner id
    /// </summary>
    /// <param name="ownerId">The owner Id to find the views for</param>
    /// <returns></returns>
    IEnumerable<MyViewInformation> GetMyViews(string ownerId);
}