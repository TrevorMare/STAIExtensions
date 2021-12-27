namespace STAIExtensions.Abstractions.Collections;

public record class ViewCollectionOptions(int? MaximumViews, int? MaximumViewsPerDataSet, bool? UseStrictViews, bool ViewsExpire, TimeSpan? ViewExpiryDate);