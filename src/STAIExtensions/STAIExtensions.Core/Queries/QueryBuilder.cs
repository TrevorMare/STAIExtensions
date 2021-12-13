namespace STAIExtensions.Core.Queries;

public class QueryBuilder : Abstractions.Queries.IQueryBuilder
{

    #region Methods

    //
    // public IEnumerable<Abstractions.Common.AzureApiDataContractSource> GetDataContractSourcesToLoad(
    //     Abstractions.Common.AzureApiDataContractSource sources)
    // {
    //     if (sources.HasFlag(Abstractions.Common.AzureApiDataContractSource.All))
    //     {
    //         foreach (var enumValue in Enum.GetValues(typeof(Abstractions.Common.AzureApiDataContractSource)))
    //         {
    //             if ((Abstractions.Common.AzureApiDataContractSource) enumValue !=
    //                 AzureApiDataContractSource.All)
    //             {
    //                 yield return (Abstractions.Common.AzureApiDataContractSource) enumValue;   
    //             }
    //         }
    //     }
    //     else
    //     {
    //         var selectedQueryTypes = sources.GetFlags().ToList();
    //         foreach (var selectedQueryType in selectedQueryTypes)
    //         {
    //             yield return (Abstractions.Common.AzureApiDataContractSource) selectedQueryType;   
    //         }
    //     }
    // }
    #endregion
    
}