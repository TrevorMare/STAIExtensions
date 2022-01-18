using System.Text.Json;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Serialization;

/// <summary>
/// Custom Deserializer to assist with the Custom Measurement
/// </summary>
public class CustomMeasurementSeriliazation : Serialization.IFieldDeserializer
{
    public object? DeserializeValue(JsonElement jsonElement)
    {
        var rawText = jsonElement.ToString() ?? "";
        return System.Text.Json.JsonSerializer.Deserialize<CustomMeasurement>(rawText);
    }
}