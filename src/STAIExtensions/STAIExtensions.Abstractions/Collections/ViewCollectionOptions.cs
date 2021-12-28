namespace STAIExtensions.Abstractions.Collections;

public record class ViewCollectionOptions(int? MaximumViews = 1000, 
    bool? UseStrictViews = false, 
    bool ViewsExpire = true, 
    TimeSpan? DefaultViewExpiryDate = default);