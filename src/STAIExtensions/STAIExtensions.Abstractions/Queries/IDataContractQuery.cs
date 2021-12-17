using STAIExtensions.Abstractions.ApiClient.Models;
using STAIExtensions.Abstractions.Serialization;

namespace STAIExtensions.Abstractions.Queries;

public interface IDataContractQuery
{

    bool Enabled { get; set; }

    string DeserializedTableName { get; set; }

    object? Tag { get; set; }

    string BuildKustoQuery();
    
    Func<ITableRowDeserializer, ApiClientQueryResultTable,
        IEnumerable<Abstractions.DataContracts.IKustoQueryContract>>? DataRowDeserializer { get; set; }
}