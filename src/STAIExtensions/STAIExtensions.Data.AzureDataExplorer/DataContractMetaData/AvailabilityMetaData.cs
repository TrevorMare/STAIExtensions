using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData
{
  
    /// <summary>
    /// Meta data class that exposes deserialization information
    /// </summary>
    public class AvailabilityMetaData : DataContractFullMetaData, IDataContractMetaData<Availability>
    {
        
        #region Members

        private readonly Dictionary<string, DataContractFieldAttribute> _mappingAttributes =
            new Dictionary<string, DataContractFieldAttribute>(StringComparer.OrdinalIgnoreCase)
            {
                { "customMeasurements", new DataContractFieldAttribute("CustomMeasurements",typeof(Serialization.CustomMeasurementSeriliazation) ) },
                { "duration", new DataContractFieldAttribute("Duration" ) },
                { "id", new DataContractFieldAttribute("Id" ) },
                { "itemCount", new DataContractFieldAttribute("ItemCount" ) },
                { "location", new DataContractFieldAttribute("Location" ) },
                { "message", new DataContractFieldAttribute("Message" ) },
                { "name", new DataContractFieldAttribute("Name" ) },
                { "performanceBucket", new DataContractFieldAttribute("PerformanceBucket" ) },
                { "size", new DataContractFieldAttribute("Size" ) },
                { "success", new DataContractFieldAttribute("Success" ) },
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