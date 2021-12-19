
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContracts
{
    public abstract class DataContract : IHasDefaultFields
    {
        
        #region Interface Properties
        [DataContractField("appId")]
        public string? AppId { get; set; }
        
        [DataContractField("application_version")]
        public string? ApplicationVersion { get; set; }

        [DataContractField("appName")]
        public string? AppName { get; set; }
        
        [DataContractField("client_Browser")]
        public string? ClientBrowser { get; set; }
        
        [DataContractField("client_City")]
        public string? ClientCity { get; set; }
        
        [DataContractField("client_CountryOrRegion")]
        public string? ClientCountryOrRegion { get; set; }
        
        [DataContractField("client_IP")]
        public string? ClientIP { get; set; }
        
        [DataContractField("client_Model")]
        public string? ClientModel { get; set; }
        
        [DataContractField("client_OS")]
        public string? ClientOS { get; set; }
        
        [DataContractField("client_StateOrProvince")]
        public string? ClientStateOrProvince { get; set; }
        
        [DataContractField("client_Type")]
        public string? ClientType { get; set; }
        
        [DataContractField("client_RoleInstance")]
        public string? ClientRoleInstance { get; set; }        
        
        [DataContractField("client_RoleName")]
        public string? ClientRoleName { get; set; }
        
        [DataContractField("customDimensions", typeof(Serialization.CustomDimensionSeriliazation))]
        public ICustomDimension? CustomDimensions { get; set; }
        
        [DataContractField("iKey")]
        public string? iKey { get; set; }
        
        [DataContractField("itemId")]
        public string? ItemId { get; set; }
        
        [DataContractField("itemType")]
        public string? ItemType { get; set; }
        
        [DataContractField("operation_Id")]
        public string? OperationId { get; set; }

        [DataContractField("operation_Name")]
        public string? OperationName { get; set; }

        [DataContractField("operation_ParentId")]
        public string? OperationParentId { get; set; }

        [DataContractField("operation_SyntheticSource")]
        public string? OperationSyntheticSource { get; set; }

        [DataContractField("sdkVersion")]
        public string? SDKVersion { get; set; }
        
        [DataContractField("session_Id")]
        public string? SessionId { get; set; }
        
        [DataContractField("timestamp")]
        public DateTime TimeStamp { get; set; }
        
        [DataContractField("user_AccountId")]
        public string? UserAccountId { get; set; }
        
        [DataContractField("user_AuthenticatedId")]
        public string? UserAuthenticatedId { get; set; }
        
        [DataContractField("user_Id")]
        public string? UserId { get; set; }
        #endregion
        
    }
}