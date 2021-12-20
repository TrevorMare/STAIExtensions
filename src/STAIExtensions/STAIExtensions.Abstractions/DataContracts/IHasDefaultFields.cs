namespace STAIExtensions.Abstractions.DataContracts;

public interface IHasDefaultFields : IDataContract, IHasCustomDimension
{

    #region Interface Properties
    string? AppId { get; set; }

    string? ApplicationVersion { get; set; }

    string? AppName { get; set; }

    string? ClientBrowser { get; set; }

    string? ClientCity { get; set; }

    string? ClientCountryOrRegion { get; set; }

    string? ClientIP { get; set; }

    string? ClientModel { get; set; }

    string? ClientOS { get; set; }

    string? ClientStateOrProvince { get; set; }

    string? ClientType { get; set; }
    
    string? ClientRoleInstance { get; set; }

    string? ClientRoleName { get; set; }

    string? iKey { get; set; }

    string? ItemId { get; set; }

    string? ItemType { get; set; }

    string? OperationId { get; set; }

    string? OperationName { get; set; }
   
    string? OperationParentId { get; set; }

    string? OperationSyntheticSource { get; set; }

    string? SDKVersion { get; set; }

    string? SessionId { get; set; }

    string? UserAccountId { get; set; }
    
    string? UserAuthenticatedId { get; set; }

    string? UserId { get; set; }

    #endregion

}