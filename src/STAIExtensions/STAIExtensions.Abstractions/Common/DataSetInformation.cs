namespace STAIExtensions.Abstractions.Common;

public record class DataSetInformation(string DataSetName, string DataSetId, string DataSetType)
{
    public string DataSetName { get; } = DataSetName;
    public string DataSetId { get; } = DataSetId;
    public string DataSetType { get; } = DataSetType;
}
