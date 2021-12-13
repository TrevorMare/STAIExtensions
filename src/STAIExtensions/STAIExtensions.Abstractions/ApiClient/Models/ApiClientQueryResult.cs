using System.Text.Json.Serialization;

namespace STAIExtensions.Abstractions.ApiClient.Models;

public class ApiClientQueryResult
{
    #region Properties

    [JsonPropertyName("tables")] public IEnumerable<ApiClientQueryResultTable>? Tables { get; set; } = null;

    #endregion
    
}