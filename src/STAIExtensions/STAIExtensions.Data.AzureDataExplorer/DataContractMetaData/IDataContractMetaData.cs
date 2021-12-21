namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData;

public interface IDataContractMetaData
{
    public Attributes.DataContractFieldAttribute? this[string columnName] { get; }
}

public interface IDataContractMetaData<T> : IDataContractMetaData
{
    
}