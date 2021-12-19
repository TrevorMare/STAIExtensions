
namespace STAIExtensions.Abstractions.Queries;

public interface IDataContractQuery
{

    bool Enabled { get; set; }

    string DeserializedTableName { get; set; }

    object? Tag { get; set; }

    string BuildKustoQuery();
    
 
}