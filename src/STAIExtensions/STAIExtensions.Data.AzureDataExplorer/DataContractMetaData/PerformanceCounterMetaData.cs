using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData
{
    public class PerformanceCounterMetaData : DataContractFullMetaData, IDataContractMetaData<PerformanceCounter>
    {
        
        #region Members

        private readonly Dictionary<string, DataContractFieldAttribute> _mappingAttributes =
            new Dictionary<string, DataContractFieldAttribute>(StringComparer.OrdinalIgnoreCase)
            {
                { "category", new DataContractFieldAttribute("Category" ) },
                { "counter", new DataContractFieldAttribute("Counter" ) },
                { "instance", new DataContractFieldAttribute("Instance" ) },
                { "name", new DataContractFieldAttribute("Name" ) },
                { "value", new DataContractFieldAttribute("Value" ) },
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