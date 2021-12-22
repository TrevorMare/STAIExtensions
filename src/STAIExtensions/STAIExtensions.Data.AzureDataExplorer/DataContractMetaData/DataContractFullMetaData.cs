using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContractMetaData
{
    public class DataContractFullMetaData : IDataContractMetaData<DataContract>
    {

        #region Members

        private readonly Dictionary<string, DataContractFieldAttribute> _mappingAttributes =
            new Dictionary<string, DataContractFieldAttribute>(StringComparer.OrdinalIgnoreCase)
            {
                { "appId", new DataContractFieldAttribute("AppId" ) },
                { "application_version", new DataContractFieldAttribute("ApplicationVersion" ) },
                { "appName", new DataContractFieldAttribute("AppName" ) },
                { "client_Browser", new DataContractFieldAttribute("ClientBrowser" ) },
                { "client_City", new DataContractFieldAttribute("ClientCity" ) },
                { "client_CountryOrRegion", new DataContractFieldAttribute("ClientCountryOrRegion" ) },
                { "client_IP", new DataContractFieldAttribute("ClientIP" ) },
                { "client_Model", new DataContractFieldAttribute("ClientModel" ) },
                { "client_OS", new DataContractFieldAttribute("ClientOS" ) },
                { "client_StateOrProvince", new DataContractFieldAttribute("ClientStateOrProvince" ) },
                { "client_Type", new DataContractFieldAttribute("ClientType" ) },
                { "cloud_RoleInstance", new DataContractFieldAttribute("CloudRoleInstance" ) },
                { "cloud_RoleName", new DataContractFieldAttribute("CloudRoleName" ) },
                { "customDimensions", new DataContractFieldAttribute("CustomDimensions", typeof(Serialization.CustomDimensionSeriliazation) ) },
                { "iKey", new DataContractFieldAttribute("iKey" ) },
                { "itemId", new DataContractFieldAttribute("ItemId" ) },
                { "itemType", new DataContractFieldAttribute("itemType" ) },
                { "operation_Id", new DataContractFieldAttribute("OperationId" ) },
                { "operation_Name", new DataContractFieldAttribute("OperationName" ) },
                { "operation_ParentId", new DataContractFieldAttribute("OperationParentId" ) },
                { "operation_SyntheticSource", new DataContractFieldAttribute("OperationSyntheticSource" ) },
                { "sdkVersion", new DataContractFieldAttribute("SDKVersion" ) },
                { "session_Id", new DataContractFieldAttribute("SessionId" ) },
                { "timestamp", new DataContractFieldAttribute("TimeStamp" ) },
                { "user_AccountId", new DataContractFieldAttribute("UserAccountId" ) },
                { "user_AuthenticatedId", new DataContractFieldAttribute("UserAuthenticatedId" ) },
                { "user_Id", new DataContractFieldAttribute("UserId" ) },
            };
        #endregion

        #region Default Accessor

        public DataContractFieldAttribute? this[string columnName]
        {
            get
            {
                if (string.IsNullOrEmpty(columnName))
                    return null;
                
                columnName = columnName.Trim();

                return _mappingAttributes.ContainsKey(columnName) ? this._mappingAttributes[columnName] : null;
            }
        }

        #endregion
        
    }
}