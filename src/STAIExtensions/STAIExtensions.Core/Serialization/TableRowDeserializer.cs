﻿using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Core.Serialization;

public class TableRowDeserializer : Abstractions.Serialization.ITableRowDeserializer
{

    #region Members

    private readonly ILogger<TableRowDeserializer> _logger;

    #endregion

    #region ctor

    public TableRowDeserializer(ILogger<TableRowDeserializer> logger = default)
    {
        this._logger = logger;
    }
    #endregion

    #region Public Methods
    public IEnumerable<T>? DeserializeTableRows<T>(Abstractions.ApiClient.Models.ApiClientQueryResultTable table)
    {
        try
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            if (string.IsNullOrEmpty(table.Rows))
            {
                this._logger?.LogTrace($"No rows found in table {table.TableName}");
                return null;
            }

            var tableColumnMapping = BuildColumnPropertyMapping<T>(table);
            if (tableColumnMapping == null)
            {
                this._logger?.LogTrace($"No table mapping built for table {table.TableName}");
                return null;
            }

            var tableRows = DeserializeTableRowsFromJson(table.Rows);
            if (tableRows == null || tableRows.Any() == false)
            {
                this._logger?.LogWarning($"Could not deserialize rows for table {table.TableName}");
                return null;
            }

            return ExtractRowsFromTableRows<T>(tableRows, tableColumnMapping.ToList());
        }
        catch (Exception ex)
        {
            this._logger?.LogError($"An Error Occured: {ex}");
            throw;
        }
    }

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
                this._logger?.LogTrace($"Extracting information from row index {iRow}");
                result.Add(ExtractRowFromTableRow<T>(row.ToList(), columnIndices.ToList()));
                iRow++;
            }
        }
        catch (Exception ex)
        {
            this._logger?.LogError($"Error on row {iRow}. {ex}");
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
                            this._logger?.LogTrace($"Unknown column type {columnData.Column.TypeName} for column {columnData.Column.ColumnName}. Trying default converter property set.");
                            columnData.Property.SetValue(instance, ConvertFromJsonObject(columnData.Property.PropertyType, jsonElement));
                        }
                    }
                    else
                    {
                        this._logger?.LogTrace($"Extracting value from custom deserializer instance.");
                        var propertyValue = columnData.FieldDeserializer.DeserializeValue(jsonElement);
                        columnData.Property.SetValue(instance, propertyValue);
                    }
                }
            }
        }
        return instance;
    }

    internal object ConvertFromJsonObject(Type targetType, JsonElement value)
    {
        object result = null;
        try
        {
            var converter = TypeDescriptor.GetConverter(targetType);
            result = converter.ConvertFromString(value.ToString());
        }
        catch (NotSupportedException ex)
        {
            this._logger?.LogError($"Unable to convert value. {ex}");
        }
        return result;
    }
    

    internal IEnumerable<TableColumnPropertyMap>? BuildColumnPropertyMapping<T>(Abstractions.ApiClient.Models.ApiClientQueryResultTable table)
    {
        if (table == null)
        {
            this._logger?.LogWarning($"Table is null, could not build column mapping.");
            return null;
        }

        if (table.Columns?.Count() == 0)
        {
            this._logger?.LogWarning($"No columns defined for table {table.TableName}. Could not build column mapping.");
            return null;
        }

        var columnIndices = table.Columns?.Select((item, index) => new { Index = index, Column = item }).ToList();
        if (columnIndices == null || columnIndices.Any() == false)
            return null;
        
        var typeProperties = typeof(T).GetProperties().ToList();
        var result = new List<TableColumnPropertyMap>();
        columnIndices.ForEach(item =>
        {
            var columnMappingName = item.Column.ColumnName;
            var propertyInfo = GetPropertyInfoByColumnName(typeProperties, columnMappingName);
            var propertyDeserializer = GetPropertyFieldDeserializer(propertyInfo);

            if (propertyInfo == null)
                this._logger?.LogWarning($"Column mapping - Could not find a related setter property.");        
            else
            {
                if (propertyDeserializer == null)
                    this._logger?.LogTrace($"Column mapping - Found related property.");
                else
                    this._logger?.LogTrace($"Column mapping - Found related property and custom deserializer.");
            }

            var mapping = new TableColumnPropertyMap(item.Index, item.Column, propertyInfo, propertyDeserializer);
            result.Add(mapping);
        });
        
        this._logger?.LogTrace($"Build mapping for {result.Count()} columns.");
        
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

    internal Abstractions.Serialization.IFieldDeserializer? GetPropertyFieldDeserializer(PropertyInfo? propertyInfo)
    {
        if (propertyInfo == null)
            return null;
        
        var customAttribute = propertyInfo.GetCustomAttribute<Abstractions.Attributes.DataContractFieldAttribute>();
        if (customAttribute != null)
            return customAttribute.FieldDeserializer;
        return null;
    }
  
    #endregion
   

}