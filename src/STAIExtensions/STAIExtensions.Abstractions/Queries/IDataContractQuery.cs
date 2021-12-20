using STAIExtensions.Abstractions.DataContracts;

namespace STAIExtensions.Abstractions.Queries;

public interface IDataContractQuery
{
    Type ContractType { get; }
        
    object? QueryParameterData { get; set; }
        
    bool Enabled { get; set; }

    object BuildQueryData();
}