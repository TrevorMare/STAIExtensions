namespace STAIExtensions.Abstractions.DataContracts.Models;

/// <summary>
/// Base Model for data returned from the telemetry source
/// </summary>
public class DataContractFull : DataContract, IHasCustomDimension
{
    
    public DateTime TimeStamp { get; set; }

    public CustomDimension? CustomDimensions { get; set; }

    public string? AppId { get; set; }
    
    public string? ApplicationVersion { get; set; }
    
    public string? AppName { get; set; }
    
    public string? ClientBrowser { get; set; }
    
    public string? ClientCity { get; set; }
    
    public string? ClientCountryOrRegion { get; set; }
    
    public string? ClientIP { get; set; }
    
    public string? ClientModel { get; set; }
    
    public string? ClientOS { get; set; }
    
    public string? ClientStateOrProvince { get; set; }
    
    public string? ClientType { get; set; }
    
    public string? CloudRoleInstance { get; set; }

    public string? CloudRoleName { get; set; }
    
    public string? iKey { get; set; }
    
    public string? ItemId { get; set; }
    
    public string? ItemType { get; set; }
    
    public string? OperationId { get; set; }
    
    public string? OperationName { get; set; }
    
    public string? OperationParentId { get; set; }
    
    public string? OperationSyntheticSource { get; set; }
    
    public string? SDKVersion { get; set; }
    
    public string? SessionId { get; set; }
    
    public string? UserAccountId { get; set; }
    
    public string? UserAuthenticatedId { get; set; }
    
    public string? UserId { get; set; }

    public string? ResourceId { get; set; }

}