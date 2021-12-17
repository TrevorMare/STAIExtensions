using STAIExtensions.Abstractions.DataContracts;

namespace STAIExtensions.Abstractions.Serialization;

public interface ITableRowDeserializer
{

    #region Methods

    IEnumerable<T>? DeserializeTableRows<T>(Abstractions.ApiClient.Models.ApiClientQueryResultTable table) where T : IKustoQueryContract;

    #endregion
    
}