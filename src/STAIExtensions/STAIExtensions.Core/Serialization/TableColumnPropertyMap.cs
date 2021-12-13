using System.Reflection;

namespace STAIExtensions.Core.Serialization;

public record class TableColumnPropertyMap(int Index, Abstractions.ApiClient.Models.ApiClientQueryResultColumn Column, PropertyInfo? Property, Abstractions.Serialization.IFieldDeserializer? FieldDeserializer);
