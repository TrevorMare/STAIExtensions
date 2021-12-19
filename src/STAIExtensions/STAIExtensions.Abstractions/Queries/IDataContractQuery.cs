using STAIExtensions.Abstractions.DataContracts;

namespace STAIExtensions.Abstractions.Queries;

public interface IDataContractQuery<T> where T : IDataContract
{
    Type ContractType { get; }
        
    object? QueryParameterData { get; set; }
        
    bool Enabled { get; set; }

    object BuildQueryData();
}