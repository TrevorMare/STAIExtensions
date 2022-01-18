using System.Text.Json;

namespace STAIExtensions.Data.AzureDataExplorer.Serialization;

/// <summary>
/// Interface for custom deserializers on property mappings
/// </summary>
public interface IFieldDeserializer
{
    object? DeserializeValue(JsonElement jsonElement);
}