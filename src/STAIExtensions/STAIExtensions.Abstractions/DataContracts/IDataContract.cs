using System;
using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts
{
    public interface IDataContract : IHasCustomDimension, IKustoQueryContract
    {

        #region Interface Properties
        [DataContractField("appId")]
        string? AppId { get; set; }
        
        [DataContractField("application_version")]
        string? ApplicationVersion { get; set; }

        [DataContractField("appName")]
        string? AppName { get; set; }
        
        [DataContractField("client_Browser")]
        string? ClientBrowser { get; set; }
        
        [DataContractField("client_City")]
        string? ClientCity { get; set; }
        
        [DataContractField("client_CountryOrRegion")]
        string? ClientCountryOrRegion { get; set; }
        
        [DataContractField("client_IP")]
        string? ClientIP { get; set; }
        
        [DataContractField("client_Model")]
        string? ClientModel { get; set; }
        
        [DataContractField("client_OS")]
        string? ClientOS { get; set; }
        
        [DataContractField("client_StateOrProvince")]
        string? ClientStateOrProvince { get; set; }
        
        [DataContractField("client_Type")]
        string? ClientType { get; set; }
        
        [DataContractField("client_RoleInstance")]
        string? ClientRoleInstance { get; set; }        
        
        [DataContractField("client_RoleName")]
        string? ClientRoleName { get; set; }
        
        [DataContractField("iKey")]
        string? iKey { get; set; }
        
        [DataContractField("itemId")]
        string? ItemId { get; set; }
        
        [DataContractField("itemType")]
        string? ItemType { get; set; }
        
        [DataContractField("operation_Id")]
        string? OperationId { get; set; }

        [DataContractField("operation_Name")]
        string? OperationName { get; set; }

        [DataContractField("operation_ParentId")]
        string? OperationParentId { get; set; }

        [DataContractField("operation_SyntheticSource")]
        string? OperationSyntheticSource { get; set; }

        [DataContractField("sdkVersion")]
        string? SDKVersion { get; set; }
        
        [DataContractField("session_Id")]
        string? SessionId { get; set; }
        
        [DataContractField("timestamp")]
        DateTime TimeStamp { get; set; }
        
        [DataContractField("user_AccountId")]
        string? UserAccountId { get; set; }
        
        [DataContractField("user_AuthenticatedId")]
        string? UserAuthenticatedId { get; set; }
        
        [DataContractField("user_Id")]
        string? UserId { get; set; }
        #endregion
        
    }
}