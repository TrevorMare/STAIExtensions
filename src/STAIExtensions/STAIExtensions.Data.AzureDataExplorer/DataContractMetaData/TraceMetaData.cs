using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData
{
    public class TraceMetaData : DataContractFullMetaData, IDataContractMetaData<Trace>
    {
        
        #region Members

        private readonly Dictionary<string, DataContractFieldAttribute> _mappingAttributes =
            new Dictionary<string, DataContractFieldAttribute>(StringComparer.OrdinalIgnoreCase)
            {
                { "customMeasurements", new DataContractFieldAttribute("CustomMeasurements",typeof(Serialization.CustomMeasurementSeriliazation) ) },
                { "itemCount", new DataContractFieldAttribute("ItemCount" ) },
                { "message", new DataContractFieldAttribute("Message" ) },
                { "severityLevel", new DataContractFieldAttribute("SeverityLevel" ) },
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