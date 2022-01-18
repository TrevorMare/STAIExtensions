using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Data.AzureDataExplorer.Queries;

/// <summary>
/// Data Contract Query that builds a Kusto query for the Telemetry Data
/// </summary>
/// <typeparam name="T"></typeparam>
public class AzureDataExplorerQuery<T> : DataContractQuery<T> where T : Abstractions.DataContracts.Models.DataContract
{

    /// <summary>
    /// Builds the query data and returns a string Kusto Query for the telemetry loader 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public override object BuildQueryData()
    {
        if (this.QueryParameterData == null)
            throw new Exception($"Unable to build query data as {nameof(QueryParameterData)} is null");

        var parameter = this.QueryParameterData as AzureDataExplorerQueryParameter;
        if (parameter == null)
            throw new Exception($"Unable to cast parameter data to {nameof(AzureDataExplorerQueryParameter)}");
        
        return parameter.BuildKustoQuery();
    }        
}