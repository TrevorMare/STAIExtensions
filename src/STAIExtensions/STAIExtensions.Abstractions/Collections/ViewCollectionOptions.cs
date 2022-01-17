namespace STAIExtensions.Abstractions.Collections;

/// <summary>
/// Options for the View Collection
/// </summary>
/// <param name="MaximumViews"><see cref="MaximumViews"/></param>
/// <param name="UseStrictViews"><see cref="UseStrictViews"/></param>
/// <param name="ViewsExpire"><see cref="ViewsExpire"/></param>
/// <param name="SlidingExpirationTimeSpan"><see cref="SlidingExpirationTimeSpan"/></param>
public record ViewCollectionOptions(int? MaximumViews = 1000, 
    bool? UseStrictViews = false, 
    bool ViewsExpire = true, 
    TimeSpan? SlidingExpirationTimeSpan = default)
{
    /// <summary>
    /// The total number of views allowed in the collection
    /// </summary>
    public int? MaximumViews { get; } = MaximumViews;
    
    /// <summary>
    /// Indicates if the Owner Id is required when interacting with the View Collection 
    /// </summary>
    public bool? UseStrictViews { get; } = UseStrictViews;
    
    /// <summary>
    /// An value to indicate if Views should expire after a certain period of in-activeness
    /// </summary>
    public bool ViewsExpire { get; } = ViewsExpire;
    
    /// <summary>
    /// Sets the maximum period of in-activeness before the view will be removed
    /// </summary>
    public TimeSpan? SlidingExpirationTimeSpan { get; } = SlidingExpirationTimeSpan;
}