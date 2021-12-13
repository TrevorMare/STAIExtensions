namespace STAIExtensions.Abstractions.Serialization;

public interface ITableRowDeserializer
{
    IEnumerable<T>? DeserializeTableRows<T>(Abstractions.ApiClient.Models.ApiClientQueryResultTable table);
}