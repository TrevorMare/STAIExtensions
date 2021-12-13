using System.Text.Json;

namespace STAIExtensions.Abstractions.DataContracts.Serialization;

public class ExceptionParsedStackSerialization : Abstractions.Serialization.IFieldDeserializer
{
    public object DeserializeValue(JsonElement jsonElement)
    {
        var rawText = jsonElement.ToString() ?? "";
        var serializationOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        
        return System.Text.Json.JsonSerializer.Deserialize<List<Models.ExceptionParsedStack>>(rawText,serializationOptions);
    }
}