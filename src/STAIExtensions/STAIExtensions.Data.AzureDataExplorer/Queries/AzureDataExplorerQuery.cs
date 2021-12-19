namespace STAIExtensions.Data.AzureDataExplorer.Queries;

public class AzureDataExplorerQuery<T> : Abstractions.Queries.DataContractQuery<T> where T : Abstractions.DataContracts.IDataContract
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