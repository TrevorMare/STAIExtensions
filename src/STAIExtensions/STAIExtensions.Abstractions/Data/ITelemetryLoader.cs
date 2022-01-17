using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Abstractions.Data;

/// <summary>
/// Defines a Telemetry Loader interface. This is used to pull data from a variety of sources
/// </summary>
public interface ITelemetryLoader
{

    /// <summary>
    /// Executes a query on a DataSource and returns a list of records that are of type DataContract
    /// </summary>
    /// <param name="query">The query to execute</param>
    /// <typeparam name="T">A DataContract model type</typeparam>
    /// <returns></returns>
    Task<IEnumerable<T>> ExecuteQueryAsync<T>(DataContractQuery<T> query) where T : DataContract;

    /// <summary>
    /// A factory used to build data Queries specific to the telemetry loader
    /// </summary>
    IDataContractQueryFactory? DataContractQueryFactory { get; set; } 

}