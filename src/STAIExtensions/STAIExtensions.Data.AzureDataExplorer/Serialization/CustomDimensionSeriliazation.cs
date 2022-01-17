using System.Text.Json;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Serialization;


/// <summary>
/// Custom Deserializer to assist with the Custom Dimension 
/// </summary>
public class CustomDimensionSeriliazation : Serialization.IFieldDeserializer
{
    public object? DeserializeValue(JsonElement jsonElement)
    {
        var rawText = jsonElement.ToString() ?? "";
        return System.Text.Json.JsonSerializer.Deserialize<CustomDimension>(rawText);
    }
}