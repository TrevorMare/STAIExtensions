using STAIExtensions.Abstractions.ApiClient.Models;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Abstractions.Serialization;
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
            var item = new DataContractQuery(source, interval, agoPeriod, topRows, orderByTimestampAsc);
            item.DataRowDeserializer = GetRowDeserializer(source);
            yield return item;
        }
    }

    public IEnumerable<Abstractions.Queries.IDataContractQuery> BuildDataContractQueries(Abstractions.Common.AzureApiDataContractSource sources, TimeSpan agoTimespan, int? topRows, bool? orderByTimestampAsc)
    {
        var loadSources = GetDataContractSources(sources);

        foreach (var source in loadSources)
        {
            var item = new DataContractQuery(source, agoTimespan, topRows, orderByTimestampAsc);
            item.DataRowDeserializer = GetRowDeserializer(source);
            yield return item;
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
  
    internal Func<ITableRowDeserializer, ApiClientQueryResultTable,
        IEnumerable<Abstractions.DataContracts.IKustoQueryContract>>? GetRowDeserializer(
        AzureApiDataContractSource source)
    {
        switch (source)
        {
            case AzureApiDataContractSource.Availability:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.Availability>(table);
                };
            case AzureApiDataContractSource.Dependency:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.Dependency>(table);
                };
            case AzureApiDataContractSource.Exception:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.Exception>(table);
                };
            case AzureApiDataContractSource.Request:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.Request>(table);
                };
            case AzureApiDataContractSource.Trace:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.Trace>(table);
                };
            case AzureApiDataContractSource.BrowserTiming:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.BrowserTiming>(table);
                };
            case AzureApiDataContractSource.CustomEvent:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.CustomEvent>(table);
                };
            case AzureApiDataContractSource.CustomMetric:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.CustomMetrics>(table);
                };
            case AzureApiDataContractSource.PageViews:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.PageView>(table);
                };
            case AzureApiDataContractSource.PerformanceCounter:
                return (deserializer, table) =>
                {
                    return deserializer.DeserializeTableRows<Abstractions.DataContracts.Models.PerformanceCounter>(table);
                };
            default:
                throw new ArgumentException("Unknown value for source");
        }
    }
    #endregion
    
}