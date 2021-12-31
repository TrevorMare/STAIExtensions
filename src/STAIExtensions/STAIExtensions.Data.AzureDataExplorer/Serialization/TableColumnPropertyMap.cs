using System.Reflection;

namespace STAIExtensions.Data.AzureDataExplorer.Serialization;

internal record class TableColumnPropertyMap(int Index, Models.ApiClientQueryResultColumn Column, PropertyInfo? Property, IFieldDeserializer? FieldDeserializer)
{
    public int Index { get; } = Index;
    public Models.ApiClientQueryResultColumn Column { get; } = Column;
    public PropertyInfo? Property { get; } = Property;
    public IFieldDeserializer? FieldDeserializer { get; } = FieldDeserializer;
}
