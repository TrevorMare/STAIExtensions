using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData
{
    
    /// <summary>
    /// Meta data class that exposes deserialization information
    /// </summary>
    public class CustomMetricMetaData : DataContractFullMetaData, IDataContractMetaData<CustomMetric>
    {
        
        #region Members

        private readonly Dictionary<string, DataContractFieldAttribute> _mappingAttributes =
            new Dictionary<string, DataContractFieldAttribute>(StringComparer.OrdinalIgnoreCase)
            {
                { "name", new DataContractFieldAttribute("Name") },
                { "value", new DataContractFieldAttribute("Value" ) },
                { "valueCount", new DataContractFieldAttribute("ValueCount" ) },
                { "valueMax", new DataContractFieldAttribute("ValueMax" ) },
                { "valueMin", new DataContractFieldAttribute("ValueMin" ) },
                { "valueStdDev", new DataContractFieldAttribute("ValueStdDev" ) },
                { "valueSum", new DataContractFieldAttribute("ValueSum" ) },
            };
        #endregion

        #region Default Accessor

        public new DataContractFieldAttribute? this[string columnName]
        {
            get
            {
                if (string.IsNullOrEmpty(columnName))
                    return null;
                
                columnName = columnName.Trim();

                return (_mappingAttributes.ContainsKey(columnName) ? this._mappingAttributes[columnName] : null) ??
                       base[columnName];
            }
        }

        #endregion

    }
}