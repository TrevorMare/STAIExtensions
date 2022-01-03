using System.ComponentModel;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Data.AzureDataExplorer.Attributes;
using STAIExtensions.Data.AzureDataExplorer.DataContractMetaData;

namespace STAIExtensions.Data.AzureDataExplorer.Serialization;

internal class TableRowDeserializer 
{

    #region Members

    protected readonly ILogger<TableRowDeserializer>? Logger;

    #endregion

    #region ctor

    public TableRowDeserializer()
    {
        this.Logger = Abstractions.DependencyExtensions.CreateLogger<TableRowDeserializer>();
    }
    #endregion

    #region Public Methods
    public IEnumerable<T>? DeserializeTableRows<T>(Models.ApiClientQueryResultTable table) where T : Abstractions.DataContracts.Models.DataContract
    {
        try
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            if (string.IsNullOrEmpty(table.Rows))
            {
                this.Logger?.LogTrace("No rows found in table {TableName}", table.TableName);
                return null;
            }

            var tableColumnMapping = BuildColumnPropertyMapping<T>(table);
            if (tableColumnMapping == null)
            {
                this.Logger?.LogTrace("No table mapping built for table {TableName}", table.TableName);
                return null;
            }

            var tableRows = DeserializeTableRowsFromJson(table.Rows);
            if (tableRows == null || tableRows.Any() == false)
            {
                this.Logger?.LogTrace("No rows returned for query on table {TableName}", table.TableName);
                return null;
            }

            return ExtractRowsFromTableRows<T>(tableRows, tableColumnMapping.ToList());
        }
        catch (Exception ex)
        {
            this.Logger?.LogError(ex, "An error occured deserializing rows");
            throw;
        }
    }
 
    #endregion

    #region Internal Methods

    internal List<IEnumerable<object>>? DeserializeTableRowsFromJson(string rawJsonRows)
    {
        var result = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<IEnumerable<object>>>(rawJsonRows);
        return result?.ToList();
    }

    internal IEnumerable<T> ExtractRowsFromTableRows<T>(IEnumerable<IEnumerable<object>> rows, List<TableColumnPropertyMap> columnIndices)
    {
        var result = new List<T>();

        var iRow = 0;

        try
        {
            foreach (var row in rows)
            {
                this.Logger?.LogTrace("Extracting information from row index {RowIndex}", iRow);
                result.Add(ExtractRowFromTableRow<T>(row.ToList(), columnIndices.ToList()));
                iRow++;
            }
        }
        catch (Exception ex)
        {
            this.Logger?.LogError(ex, "Error on row {RowIndex}", iRow);
            throw;
        }
        return result;
    }

    internal T ExtractRowFromTableRow<T>(List<object> rowValues, List<TableColumnPropertyMap> columnIndices)
    {
        T instance = Activator.CreateInstance<T>();
        
        foreach (var columnData in columnIndices)
        {
            // Check if the row values are in bound of the column data index and that there is a mapping property             
            if (rowValues.Count() - 1 > columnData.Index && columnData.Property != null)
            {
                if (rowValues[columnData.Index] == null)
                {
                    continue;
                }
                
                // Get the row value object, this is a json token value
                var jsonElement = (JsonElement)rowValues[columnData.Index];
                if (!string.IsNullOrEmpty(jsonElement.GetRawText()))
                {
                    // Check if there is a custom deserializer attached
                    if (columnData.FieldDeserializer == null)
                    {
                        // Try the internal deserializer
                        if (columnData.Column.TypeName.ToLower() == "string")
                        {
                            columnData.Property.SetValue(instance, jsonElement.GetString());
                        }
                        else if (columnData.Column.TypeName.ToLower() == "int")
                        {
                            columnData.Property.SetValue(instance, jsonElement.GetInt32());
                        }
                        else if (columnData.Column.TypeName.ToLower() == "real")
                        {
                            columnData.Property.SetValue(instance, jsonElement.GetDouble());
                        }
                        else if (columnData.Column.TypeName.ToLower() == "datetime")
                        {
                            columnData.Property.SetValue(instance, jsonElement.GetDateTime());
                        }
                        else
                        {
                            this.Logger?.LogTrace($"Unknown column type {columnData.Column.TypeName} for column {columnData.Column.ColumnName}. Trying default converter property set.");
                            columnData.Property.SetValue(instance, ConvertFromJsonObject(columnData.Property.PropertyType, jsonElement));
                        }
                    }
                    else
                    {
                        this.Logger?.LogTrace($"Extracting value from custom deserializer instance.");
                        var propertyValue = columnData.FieldDeserializer.DeserializeValue(jsonElement);
                        columnData.Property.SetValue(instance, propertyValue);
                    }
                }
            }
        }
        return instance;
    }

    internal object? ConvertFromJsonObject(Type targetType, JsonElement value)
    {
        object result = null;
        try
        {
            var converter = TypeDescriptor.GetConverter(targetType);
            result = converter.ConvertFromString(value.ToString());
        }
        catch (NotSupportedException ex)
        {
            this.Logger?.LogError(ex, $"Unable to convert value");
        }
        return result;
    }
    
    internal IEnumerable<TableColumnPropertyMap>? BuildColumnPropertyMapping<T>(Models.ApiClientQueryResultTable? table)
    {
        if (table == null)
        {
            this.Logger?.LogWarning($"Table is null, could not build column mapping.");
            return null;
        }

        if (table.Columns?.Count() == 0)
        {
            this.Logger?.LogWarning("No columns defined for table {TableName}. Could not build column mapping", table.TableName);
            return null;
        }

        var columnIndices = table.Columns?.Select((item, index) => new { Index = index, Column = item }).ToList();
        if (columnIndices == null || columnIndices.Any() == false)
            return null;

        var dataContractTypeMetaData = GetDataContractMetaData<T>();
        
        var typeProperties = typeof(T).GetProperties().ToList();
        var result = new List<TableColumnPropertyMap>();
        columnIndices.ForEach(item =>
        {
            var columnMappingName = item.Column.ColumnName;
            var propertyInfo = GetPropertyInfoByColumnName(typeProperties, columnMappingName, dataContractTypeMetaData);
            var propertyDeserializer = GetPropertyFieldDeserializer(propertyInfo, item.Column.ColumnName, dataContractTypeMetaData);

            if (propertyInfo == null)
                this.Logger?.LogWarning("Column mapping - Could not find a related setter property for mapping name {MappingName}", columnMappingName);        
            else
            {
                if (propertyDeserializer == null)
                    this.Logger?.LogTrace($"Column mapping - Found related property.");
                else
                    this.Logger?.LogTrace($"Column mapping - Found related property and custom deserializer.");
            }

            var mapping = new TableColumnPropertyMap(item.Index, item.Column, propertyInfo, propertyDeserializer);
            result.Add(mapping);
        });
        
        this.Logger?.LogTrace("Build mapping for {RowCount} columns", result.Count);
        
        return result;
    }

    internal PropertyInfo? GetPropertyInfoByColumnName(List<PropertyInfo>? properties, string columnName, IDataContractMetaData? dataContractMetaData = default)
    {
        if (properties == null)
            return null;
        
        if (string.IsNullOrEmpty(columnName))
            return null;
        
        // Find by name 
        var result = properties.FirstOrDefault(x => x.Name.ToLower().Trim() == columnName.ToLower().Trim());
        if (result == null)
        {
            // Find by internal mapping

            if (dataContractMetaData != null)
            {
                var customAttribute = dataContractMetaData[columnName];
                if (customAttribute != null)
                    result = properties.FirstOrDefault(x => x.Name.ToLower().Trim() == customAttribute.DeserializeFieldName.ToLower().Trim());
            }

            if (result != null)
                return result;
            
            // Find by the custom attributes
            result = (from p in properties
                let attr = p.GetCustomAttribute<DataContractFieldAttribute>()
                where attr != null && attr.DeserializeFieldName.ToLower().Trim() == columnName.ToLower().Trim()
                select p).FirstOrDefault();
        }

        if (result != null && result.CanWrite == false)
            return null;
        
        return result;
    }

    internal IFieldDeserializer? GetPropertyFieldDeserializer(PropertyInfo? propertyInfo, string columnName, IDataContractMetaData? dataContractMetaData = default)
    {
        if (propertyInfo == null && dataContractMetaData == null)
            return null;

        DataContractFieldAttribute? customAttribute = null;
        
        if (propertyInfo != null)
        {
            customAttribute = propertyInfo.GetCustomAttribute<DataContractFieldAttribute>();
            if (customAttribute?.FieldDeserializer != null)
                return customAttribute.FieldDeserializer;
        }

        if (dataContractMetaData == null) return null;
        
        customAttribute = dataContractMetaData[columnName];
        return customAttribute?.FieldDeserializer;
    }

    internal IDataContractMetaData<T>? GetDataContractMetaData<T>()
    {
        var type = typeof(IDataContractMetaData<T>);
        
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));
        
        var dataContractMetaData = types.FirstOrDefault();
        
        if (dataContractMetaData != null)
            return (IDataContractMetaData<T>)Activator.CreateInstance(dataContractMetaData);

        return null;
    }
    
    #endregion

}