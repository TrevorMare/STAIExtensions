using System.Text.Json;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Serialization;


/// <summary>
/// Custom Deserializer to assist with the Exception Stack Trace
/// </summary>
public class ExceptionParsedStackSerialization : Serialization.IFieldDeserializer
{
    public object? DeserializeValue(JsonElement jsonElement)
    {
        var rawText = jsonElement.ToString() ?? "";
        var serializationOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        
        return System.Text.Json.JsonSerializer.Deserialize<List<ExceptionParsedStack>>(rawText,serializationOptions);
    }
}