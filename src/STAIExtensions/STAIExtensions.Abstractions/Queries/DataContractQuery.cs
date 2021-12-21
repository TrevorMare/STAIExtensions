using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.Queries;

public abstract class DataContractQuery<T> : IDataContractQuery where T : DataContract
{

    public object? QueryParameterData { get; set; }
        
    public Type ContractType => typeof(T);

    public bool Enabled { get; set; } = true;

    public abstract object BuildQueryData();

}