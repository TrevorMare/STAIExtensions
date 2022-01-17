using STAIExtensions.Abstractions.DataContracts;

namespace STAIExtensions.Abstractions.Queries;

/// <summary>
/// Interface that defines the Telemetry Loader Query
/// </summary>
public interface IDataContractQuery
{
    Type ContractType { get; }
        
    object? QueryParameterData { get; set; }
        
    bool Enabled { get; set; }

    object BuildQueryData();
}