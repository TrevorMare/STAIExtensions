using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData
{
    /// <summary>
    /// Meta data class that exposes deserialization information
    /// </summary>
    public class BrowserTimingMetaData : DataContractFullMetaData, IDataContractMetaData<BrowserTiming>
    {
        #region Members

        private readonly Dictionary<string, DataContractFieldAttribute> _mappingAttributes =
            new Dictionary<string, DataContractFieldAttribute>(StringComparer.OrdinalIgnoreCase)
            {
                { "customMeasurements", new DataContractFieldAttribute("CustomMeasurements",typeof(Serialization.CustomMeasurementSeriliazation) ) },
                { "itemCount", new DataContractFieldAttribute("ItemCount" ) },
                { "name", new DataContractFieldAttribute("Name" ) },
                { "networkDuration", new DataContractFieldAttribute("NetworkDuration" ) },
                { "performanceBucket", new DataContractFieldAttribute("PerformanceBucket" ) },
                { "processingDuration", new DataContractFieldAttribute("ProcessingDuration" ) },
                { "receiveDuration", new DataContractFieldAttribute("ReceiveDuration" ) },
                { "sendDuration", new DataContractFieldAttribute("SendDuration" ) },
                { "totalDuration", new DataContractFieldAttribute("TotalDuration" ) },
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