namespace STAIExtensions.Abstractions.Common;

/// <summary>
/// Structure that is returned with the DataSet information
/// </summary>
/// <param name="DataSetName">The name of the DataSet</param>
/// <param name="DataSetId">The unique Id of the DataSet</param>
/// <param name="DataSetType">The fully qualified type name of the DataSet</param>
public record DataSetInformation(string DataSetName, string DataSetId, string DataSetType)
{
    /// <summary>
    /// Gets the name of the DataSet
    /// </summary>
    public string DataSetName { get; } = DataSetName;
    
    /// <summary>
    /// The unique Id of the DataSet
    /// </summary>
    public string DataSetId { get; } = DataSetId;
    
    /// <summary>
    /// The fully qualified type name of the DataSet
    /// </summary>
    public string DataSetType { get; } = DataSetType;
}
