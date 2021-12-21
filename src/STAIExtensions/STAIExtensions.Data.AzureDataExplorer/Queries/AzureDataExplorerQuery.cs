using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Data.AzureDataExplorer.Queries;

public class AzureDataExplorerQuery<T> : DataContractQuery<T> where T : Abstractions.DataContracts.Models.DataContract
{
        
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