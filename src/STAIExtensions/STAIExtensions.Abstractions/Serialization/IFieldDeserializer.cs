using System.Text.Json;

namespace STAIExtensions.Abstractions.Serialization;

public interface IFieldDeserializer
{
    object DeserializeValue(JsonElement jsonElement);
}