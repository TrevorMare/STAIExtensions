using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Abstractions.Data;

public interface ITelemetryLoader
{

    Task<IEnumerable<T>> ExecuteQueryAsync<T>(DataContractQuery<T> query) where T : DataContract;

    IDataContractQueryFactory? DataContractQueryFactory { get; set; } 

}