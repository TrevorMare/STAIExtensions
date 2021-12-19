using STAIExtensions.Abstractions.DataContracts;

namespace STAIExtensions.Abstractions.Queries;

public abstract class DataContractQuery<T> : IDataContractQuery<T> where T : IDataContract
{

    public object? QueryParameterData { get; set; }
        
    public Type ContractType => typeof(T);
       
    public bool Enabled { get; set; }

    public abstract object BuildQueryData();
    
}