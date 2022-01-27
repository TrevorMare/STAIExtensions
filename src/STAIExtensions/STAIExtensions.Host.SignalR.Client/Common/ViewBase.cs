using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Host.SignalR.Client.Common;

/// <summary>
/// Model that exposes common properties on a view
/// </summary>
public class ViewBase 
{
    /// <summary>
    /// Gets or sets the Id of the view
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// Gets or sets the owner of the view
    /// </summary>
    public string? OwnerId { get; set; }
    
    /// <summary>
    /// Gets the time when the view will expire
    /// </summary>
    public DateTime? ExpiryDate { get; set;}
    
    /// <summary>
    /// Gets the last updated time of the view
    /// </summary>
    public DateTime? LastUpdate { get; set;}
    
    /// <summary>
    /// Gets the fully qualified view type name 
    /// </summary>
    public string ViewTypeName { get; set;}
    
    /// <summary>
    /// Gets the fully qualified view type name 
    /// </summary>
    public string FriendlyViewTypeName { get; set;}
    
    /// <summary>
    /// Gets a value indicating if the view is enabled
    /// </summary>
    public bool RefreshEnabled { get; set;}
    
    /// <summary>
    /// Gets a list of parameters that can be set on the view
    /// </summary>
    public IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors { get; set;}
    
    /// <summary>
    /// Gets the sliding expiration of the view
    /// </summary>
    public TimeSpan SlidingExpiration { get; set; }

}