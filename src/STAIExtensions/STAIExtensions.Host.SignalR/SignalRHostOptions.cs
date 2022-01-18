namespace STAIExtensions.Host.SignalR;

/// <summary>
/// Options for the SignalR host
/// </summary>
/// <param name="UseDefaultAuthorization"></param>
/// <param name="BearerToken"></param>
public record SignalRHostOptions(bool UseDefaultAuthorization = true, string? BearerToken = "1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495")
{
    /// <summary>
    /// Gets or sets a value indicating if Authorization should be used
    /// </summary>
    public bool UseDefaultAuthorization { get; set; } = UseDefaultAuthorization;
    
    /// <summary>
    /// Gets or sets the token that should be used for authorization
    /// </summary>
    public string? BearerToken { get; set; } = BearerToken;
}
