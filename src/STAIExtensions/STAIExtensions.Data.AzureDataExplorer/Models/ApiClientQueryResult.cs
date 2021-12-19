using System.Text.Json.Serialization;

namespace STAIExtensions.Data.AzureDataExplorer.Models;

internal class ApiClientQueryResult
{
    #region Properties

    [JsonPropertyName("tables")] public IEnumerable<ApiClientQueryResultTable>? Tables { get; set; } = null;

    #endregion
    
}