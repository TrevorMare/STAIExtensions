using System.Text.Json.Serialization;

namespace STAIExtensions.Abstractions.ApiClient.Models;

public class ApiClientQueryResultColumn
{

    #region Properties
    [JsonPropertyName("name")] public string ColumnName { get; set; } = "";

    [JsonPropertyName("type")] public string TypeName { get; set; } = "";
    #endregion

}