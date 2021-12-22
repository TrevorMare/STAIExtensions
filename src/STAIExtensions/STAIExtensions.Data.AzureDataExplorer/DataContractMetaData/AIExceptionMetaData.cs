using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData
{
    public class AIExceptionMetaData : DataContractFullMetaData, IDataContractMetaData<AIException>
    {

        #region Members

        private readonly Dictionary<string, DataContractFieldAttribute> _mappingAttributes =
            new Dictionary<string, DataContractFieldAttribute>(StringComparer.OrdinalIgnoreCase)
            {
                { "assembly", new DataContractFieldAttribute("Assembly") },
                { "customMeasurements", new DataContractFieldAttribute("CustomMeasurements",typeof(Serialization.CustomMeasurementSeriliazation) ) },
                { "details", new DataContractFieldAttribute("Details",typeof(Serialization.ExceptionParsedStackSerialization) ) },
                { "handledAt", new DataContractFieldAttribute("HandledAt") },
                { "innermostAssembly" , new DataContractFieldAttribute("InnermostAssembly") },
                { "innermostMessage" , new DataContractFieldAttribute("InnermostMessage") },
                { "innermostMethod" , new DataContractFieldAttribute("InnermostMethod") },
                { "innermostType" , new DataContractFieldAttribute("InnermostType") },
                { "itemCount" , new DataContractFieldAttribute("ItemCount") },
                { "message" , new DataContractFieldAttribute("Message") },
                { "method" , new DataContractFieldAttribute("Method") },
                { "outerAssembly" , new DataContractFieldAttribute("OuterAssembly") },
                { "outerMessage" , new DataContractFieldAttribute("OuterMessage") },
                { "outerMethod" , new DataContractFieldAttribute("OuterMethod") },
                { "outerType" , new DataContractFieldAttribute("OuterType") },
                { "problemId" , new DataContractFieldAttribute("ProblemId") },
                { "severityLevel" , new DataContractFieldAttribute("SeverityLevel") },
                { "type" , new DataContractFieldAttribute("Type") },
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