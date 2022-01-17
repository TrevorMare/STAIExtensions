using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.Queries;

/// <summary>
/// Abstract class for the telemetry query 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class DataContractQuery<T> : IDataContractQuery where T : DataContract
{
    
    /// <summary>
    /// Gets or sets the additional query data that can be passed to the query to assist in building the query 
    /// </summary>
    public object? QueryParameterData { get; set; }
        
    /// <summary>
    /// The type of the record returned by the Query
    /// </summary>
    public Type ContractType => typeof(T);

    /// <summary>
    /// Gets or sets if the query is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Builds an object that the Telemetry Client has knowledge of to execute the query 
    /// </summary>
    /// <returns></returns>
    public abstract object BuildQueryData();

}