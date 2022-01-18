namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData;

/// <summary>
/// Interface for the Data Contract Entity Meta Data
/// </summary>
public interface IDataContractMetaData
{
    public Attributes.DataContractFieldAttribute? this[string columnName] { get; }
}

/// <summary>
/// Interface for the Data Contract Entity Meta Data
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDataContractMetaData<T> : IDataContractMetaData
{
    
}