using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.Views;

/// <summary>
/// Defines the interface for a View
/// </summary>
public interface IDataSetView : IDisposable
{

    /// <summary>
    /// Gets the Auto Generated Id of the View
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets or sets the Owner Id of the View
    /// </summary>
    string? OwnerId { get; set; }

    /// <summary>
    /// Gets the calculated Expiry date of the view
    /// </summary>
    DateTime? ExpiryDate { get; }
    
    /// <summary>
    /// Gets the last time the View was updated
    /// </summary>
    DateTime? LastUpdate { get; }
    
    /// <summary>
    /// Gets the fully qualified type name of the view 
    /// </summary>
    string ViewTypeName { get; }

    /// <summary>
    /// Gets the fully qualified type name of the view 
    /// </summary>
    string FriendlyViewTypeName { get; }
    
    /// <summary>
    /// Gets a value indicating if the View is frozen for updates
    /// </summary>
    bool RefreshEnabled { get; }

    /// <summary>
    /// Gets the allowed parameters description that this view accepts
    /// </summary>
    IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors { get; }

    /// <summary>
    /// Gets or sets the Sliding Expiration of the view for expiry
    /// </summary>
    TimeSpan SlidingExpiration { get; set; }
    
    /// <summary>
    /// Callback event when a view has been updated by DataSets
    /// </summary>
    event EventHandler OnViewUpdated;

    /// <summary>
    /// Updates the View from a DataSet
    /// </summary>
    /// <param name="dataset">The DataSet to use as a source to update the view from</param>
    /// <returns></returns>
    Task UpdateViewFromDataSet(IDataSet dataset);

    /// <summary>
    /// Calculates the Next expiry time from the sliding expiry time span
    /// </summary>
    void SetExpiryDate();

    /// <summary>
    /// Explicitly set the expiry date of the view
    /// </summary>
    /// <param name="value">An absolute expiry date</param>
    void SetExpiryDate(DateTime value);

    /// <summary>
    /// Sets the view parameters
    /// </summary>
    /// <param name="parameters"></param>
    void SetViewParameters(Dictionary<string, object>? parameters);

    /// <summary>
    /// Freezes the view for further updates
    /// </summary>
    void SetViewRefreshEnabled();
    
    /// <summary>
    /// Un-Freezes the view for further updates
    /// </summary>
    void SetViewRefreshDisabled();
}