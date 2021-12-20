using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Abstractions.Data;

public interface ITelemetryLoader
{

    Task<IEnumerable<T>> ExecuteQueryAsync<T>(IDataContractQuery<T> query) where T : IDataContract;

    IDataContractQueryFactory? DataContractQueryFactory { get; set; } 

}