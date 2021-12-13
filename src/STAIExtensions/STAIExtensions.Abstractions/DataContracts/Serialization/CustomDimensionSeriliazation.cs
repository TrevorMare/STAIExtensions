﻿using System.Text.Json;

namespace STAIExtensions.Abstractions.DataContracts.Serialization;

public class CustomDimensionSeriliazation : Abstractions.Serialization.IFieldDeserializer
{
    public object DeserializeValue(JsonElement jsonElement)
    {
        var rawText = jsonElement.ToString() ?? "";
        return System.Text.Json.JsonSerializer.Deserialize<Models.CustomDimension>(rawText);
    }
}