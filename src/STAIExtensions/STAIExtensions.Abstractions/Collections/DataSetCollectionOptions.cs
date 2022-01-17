namespace STAIExtensions.Abstractions.Collections;

/// <summary>
/// Options for the Data Set Collection.
/// </summary>
/// <param name="MaximumDataSets"><see cref="MaximumDataSets"/></param>
/// <param name="MaximumViewsPerDataSet"><see cref="MaximumViewsPerDataSet"/></param>
public record class DataSetCollectionOptions(int? MaximumDataSets = default,    
    int? MaximumViewsPerDataSet = default)
{
    /// <summary>
    /// The maximum allowed number of DataSets that can be handled by the system. Set a value to limit memory usage 
    /// </summary>
    public int? MaximumDataSets { get; } = MaximumDataSets;
    
    /// <summary>
    /// The maximum number of allowed views that can be attached to any DataSet
    /// </summary>
    public int? MaximumViewsPerDataSet { get; } = MaximumViewsPerDataSet;
}