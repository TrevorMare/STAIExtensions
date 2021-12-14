using STAIExtensions.Abstractions.Common;
using STAIExtensions.Core.Queries.Models;

namespace STAIExtensions.Core.Queries;

public class QueryBuilder : Abstractions.Queries.IQueryBuilder
{

    #region Methods
    public IEnumerable<Abstractions.Queries.IDataContractQuery> BuildDataContractQueries(Abstractions.Common.AzureApiDataContractSource sources, int interval, AgoPeriod agoPeriod, int? topRows, bool? orderByTimestampAsc)
    {
        var loadSources = GetDataContractSources(sources);

        foreach (var source in loadSources)
        {
            yield return new DataContractQuery(source, interval, agoPeriod, topRows, orderByTimestampAsc);
        }
    }

    public IEnumerable<Abstractions.Queries.IDataContractQuery> BuildDataContractQueries(Abstractions.Common.AzureApiDataContractSource sources, TimeSpan agoTimespan, int? topRows, bool? orderByTimestampAsc)
    {
        var loadSources = GetDataContractSources(sources);

        foreach (var source in loadSources)
        {
            yield return new DataContractQuery(source, agoTimespan, topRows, orderByTimestampAsc);
        }
    }
    
    internal IEnumerable<AzureApiDataContractSource> GetDataContractSources(AzureApiDataContractSource sources)
    {
        if (sources.HasFlag(AzureApiDataContractSource.All))
        {
            foreach (var enumValue in Enum.GetValues(typeof(AzureApiDataContractSource)))
            {
                if ((AzureApiDataContractSource) enumValue !=
                    AzureApiDataContractSource.All)
                {
                    yield return (AzureApiDataContractSource) enumValue;   
                }
            }
        }
        else
        {
            var selectedQueryTypes = sources.GetFlags().ToList();
            foreach (var selectedQueryType in selectedQueryTypes)
            {
                yield return (Abstractions.Common.AzureApiDataContractSource) selectedQueryType;   
            }
        }
    }
    #endregion
    
}