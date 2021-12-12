using System.ComponentModel;
using System.Reflection;

namespace STAIExtensions.Core.Serialization;

public class TableRowDeserializer : Abstractions.Serialization.ITableRowDeserializer
{

    #region Public Methods
    public IEnumerable<T>? DeserializeTableRows<T>(Abstractions.ApiClient.Models.ApiClientQueryResultTable table)
    {
        if (table == null)
            throw new ArgumentNullException(nameof(table));

        if (string.IsNullOrEmpty(table.Rows))
            return null;

        var tableColumnMapping = BuildColumnPropertyMapping<T>(table);
        if (tableColumnMapping == null)
            return null;

        var tableRows = DeserializeTableRowsFromJson(table.Rows);
        if (tableRows == null || tableRows.Any() == false)
            return null;

        return ExtractRowsFromTableRows<T>(tableRows, tableColumnMapping.ToList());
    }

    internal List<IEnumerable<object>>? DeserializeTableRowsFromJson(string rawJsonRows)
    {
        var result = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<IEnumerable<object>>>(rawJsonRows);
        return result?.ToList();
    }

    internal IEnumerable<T> ExtractRowsFromTableRows<T>(IEnumerable<IEnumerable<object>> rows, List<TableColumnPropertyMap> columnIndices)
    {
        var result = new List<T>();

        foreach (var row in rows)
        {
            result.Add(ExtractRowFromTableRow<T>(row.ToList(), columnIndices.ToList()));
        }
        
        return result;
    }

    internal T ExtractRowFromTableRow<T>(List<object> row, List<TableColumnPropertyMap> columnIndices)
    {
        T instance = Activator.CreateInstance<T>();
        
        foreach (var columnIndexObject in columnIndices)
        {
            // TODO: Implement robust logic to deserialize the row data
            
            if (row.Count() - 1 > columnIndexObject.Index && columnIndexObject.Property != null)
            {
                var rowValue = row[columnIndexObject.Index];

                if (rowValue != null)
                {
                    var propertyType = columnIndexObject.Property.PropertyType;
                
                    try
                    {
                        var converter = TypeDescriptor.GetConverter(propertyType);
                        if (converter != null)
                        {
                            // Cast ConvertFromString(string text) : object to (T)
                            columnIndexObject.Property.SetValue(instance, converter.ConvertFromString(rowValue?.ToString()));
                        }
                        else
                        {
                            columnIndexObject.Property.SetValue(instance, rowValue);  
                        }
                    }
                    catch (NotSupportedException)
                    {
                        columnIndexObject.Property.SetValue(instance, rowValue);
                    }
                    
                }
            }
        }
        return instance;
    }

    internal IEnumerable<TableColumnPropertyMap>? BuildColumnPropertyMapping<T>(Abstractions.ApiClient.Models.ApiClientQueryResultTable table)
    {
        if (table.Columns?.Count() == 0)
            return null;
        
        var columnIndices = table.Columns?.Select((item, index) => new { Index = index, Column = item }).ToList();
        if (columnIndices == null || columnIndices.Any() == false)
            return null;
        
        var typeProperties = typeof(T).GetProperties().ToList();
        var result = new List<TableColumnPropertyMap>();
        columnIndices.ForEach(item =>
        {
            var columnMappingName = item.Column.ColumnName;
            var propertyInfo = GetPropertyInfoByColumnName(typeProperties, columnMappingName);

            var mapping = new TableColumnPropertyMap(item.Index, item.Column, propertyInfo);
            result.Add(mapping);
        });
        
        return result;
    }

    internal PropertyInfo? GetPropertyInfoByColumnName(List<PropertyInfo>? properties, string columnName)
    {
        if (properties == null)
            return null;
        
        if (string.IsNullOrEmpty(columnName))
            return null;
        
        // Find by name 
        var result = properties.FirstOrDefault(x => x.Name.ToLower().Trim() == columnName.ToLower().Trim());
        if (result == null)
        {
            // Find by the custom attributes
            result = (from p in properties
                let attr = p.GetCustomAttribute<Abstractions.Attributes.DataContractFieldAttribute>()
                where attr != null && attr.DeserializeFieldName.ToLower().Trim() == columnName.ToLower().Trim()
                select p).FirstOrDefault();
        }

        if (result != null && result.CanWrite == false)
            return null;
        
        return result;
    }
  
    #endregion
   

}