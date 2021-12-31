namespace STAIExtensions.Abstractions.Collections;

public record class DataSetCollectionOptions(int? MaximumDataSets = default,    
    int? MaximumViewsPerDataSet = default)
{
    public int? MaximumDataSets { get; } = MaximumDataSets;
    public int? MaximumViewsPerDataSet { get; } = MaximumViewsPerDataSet;
}
