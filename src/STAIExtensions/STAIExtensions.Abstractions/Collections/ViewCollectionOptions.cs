namespace STAIExtensions.Abstractions.Collections;

public record class ViewCollectionOptions(int? MaximumViews = 1000, 
    bool? UseStrictViews = false, 
    bool ViewsExpire = true, 
    TimeSpan? SlidingExpirationTimeSpan = default)
{
    public int? MaximumViews { get; } = MaximumViews;
    public bool? UseStrictViews { get; } = UseStrictViews;
    public bool ViewsExpire { get; } = ViewsExpire;
    public TimeSpan? SlidingExpirationTimeSpan { get; } = SlidingExpirationTimeSpan;
}