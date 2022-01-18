using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData
{
    
    /// <summary>
    /// Meta data class that exposes deserialization information
    /// </summary>
    public class PageViewMetaData : DataContractFullMetaData, IDataContractMetaData<PageView>
    {
        
        #region Members

        private readonly Dictionary<string, DataContractFieldAttribute> _mappingAttributes =
            new Dictionary<string, DataContractFieldAttribute>(StringComparer.OrdinalIgnoreCase)
            {
                { "customMeasurements", new DataContractFieldAttribute("CustomMeasurements",typeof(Serialization.CustomMeasurementSeriliazation) ) },
                { "duration", new DataContractFieldAttribute("Duration" ) },
                { "id", new DataContractFieldAttribute("Id" ) },
                { "itemCount", new DataContractFieldAttribute("ItemCount" ) },
                { "name", new DataContractFieldAttribute("Name" ) },
                { "performanceBucket", new DataContractFieldAttribute("PerformanceBucket" ) },
                { "url", new DataContractFieldAttribute("Url" ) },
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