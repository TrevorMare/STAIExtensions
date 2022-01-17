namespace STAIExtensions.Abstractions.Collections;

/// <summary>
/// Options for the Data Set Collection.
/// </summary>
public class DataSetCollectionOptions
{
    /// <summary>
    /// The maximum allowed number of DataSets that can be handled by the system. Set a value to limit memory usage
    /// </summary>
    public int? MaximumDataSets { get; } 
    
    /// <summary>
    /// The maximum number of allowed views that can be attached to any DataSet
    /// </summary>
    public int? MaximumViewsPerDataSet { get; } 

    public DataSetCollectionOptions(int? maximumDataSets = default, int? maximumViewsPerDataSet = default)
    {
        this.MaximumDataSets = maximumDataSets;
        this.MaximumViewsPerDataSet = maximumViewsPerDataSet;
    }
}
