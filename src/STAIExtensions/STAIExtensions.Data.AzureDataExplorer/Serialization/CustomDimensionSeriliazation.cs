﻿using System.Text.Json;
using STAIExtensions.Data.AzureDataExplorer.DataContracts;

namespace STAIExtensions.Data.AzureDataExplorer.Serialization;

public class CustomDimensionSeriliazation : Serialization.IFieldDeserializer
{
    public object DeserializeValue(JsonElement jsonElement)
    {
        var rawText = jsonElement.ToString() ?? "";
        return System.Text.Json.JsonSerializer.Deserialize<CustomDimension>(rawText);
    }
}