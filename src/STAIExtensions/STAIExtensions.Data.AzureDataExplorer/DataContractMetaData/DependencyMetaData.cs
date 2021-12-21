using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData
{
    public class DependencyMetaData : DataContractFullMetaData, IDataContractMetaData<Dependency>
    {
        
        #region Members

        private readonly Dictionary<string, DataContractFieldAttribute> _mappingAttributes =
            new Dictionary<string, DataContractFieldAttribute>(StringComparer.OrdinalIgnoreCase)
            {
                { "customMeasurements", new DataContractFieldAttribute("CustomMeasurements",typeof(Serialization.CustomMeasurementSeriliazation) ) },
                { "data", new DataContractFieldAttribute("Data" ) },
                { "duration", new DataContractFieldAttribute("Duration" ) },
                { "id", new DataContractFieldAttribute("Id" ) },
                { "itemCount", new DataContractFieldAttribute("ItemCount" ) },
                { "name", new DataContractFieldAttribute("Name" ) },
                { "performanceBucket", new DataContractFieldAttribute("PerformanceBucket" ) },
                { "resultCode", new DataContractFieldAttribute("ResultCode" ) },
                { "success", new DataContractFieldAttribute("Success" ) },
                { "target", new DataContractFieldAttribute("Target" ) },
                { "type", new DataContractFieldAttribute("Type" ) },
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