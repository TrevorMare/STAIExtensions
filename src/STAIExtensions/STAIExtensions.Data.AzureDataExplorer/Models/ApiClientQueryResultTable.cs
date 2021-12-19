using System.Text.Json.Serialization;
using STAIExtensions.Data.AzureDataExplorer.JsonHelpers;

namespace STAIExtensions.Data.AzureDataExplorer.Models;

internal class ApiClientQueryResultTable
{

    #region Properties

    [JsonPropertyName("name")] public string TableName { get; set; } = "";

    [JsonPropertyName("columns")]
    public IEnumerable<ApiClientQueryResultColumn>? Columns { get; set; } = null;

    [JsonPropertyName("rows")]     
    [JsonConverter(typeof(ApiClientQueryResultTableRowConverter))]
    public string Rows { get; set; } = "";
    #endregion

}