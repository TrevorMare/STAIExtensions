using System.Reflection;

namespace STAIExtensions.Data.AzureDataExplorer.Serialization;

internal record class TableColumnPropertyMap(int Index, Models.ApiClientQueryResultColumn Column, PropertyInfo? Property, IFieldDeserializer? FieldDeserializer);
