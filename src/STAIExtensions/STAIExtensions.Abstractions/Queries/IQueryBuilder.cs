using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Abstractions.Queries;

public interface IQueryBuilder
{

    IEnumerable<Abstractions.Queries.IDataContractQuery> BuildDataContractQueries(
        Abstractions.Common.AzureApiDataContractSource sources, int interval, AgoPeriod agoPeriod, int? topRows,
        bool? orderByTimestampAsc);

    IEnumerable<Abstractions.Queries.IDataContractQuery> BuildDataContractQueries(
        Abstractions.Common.AzureApiDataContractSource sources, TimeSpan agoTimespan, int? topRows,
        bool? orderByTimestampAsc);


}