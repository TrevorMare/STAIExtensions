using System.Text.Json;

namespace STAIExtensions.Data.AzureDataExplorer.Serialization;

public interface IFieldDeserializer
{
    object DeserializeValue(JsonElement jsonElement);
}