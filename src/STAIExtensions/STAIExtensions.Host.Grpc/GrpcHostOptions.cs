namespace STAIExtensions.Host.Grpc;

/// <summary>
/// Grpc Hosting Options
/// </summary>
/// <param name="UseDefaultAuthorization"></param>
/// <param name="BearerToken"></param>
public record GrpcHostOptions(bool UseDefaultAuthorization = true, string? BearerToken = "598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666")
{
    /// <summary>
    /// Gets or sets a value indicating if the default authorization should be enabled
    /// </summary>
    public bool UseDefaultAuthorization { get; set; } = UseDefaultAuthorization;
    
    /// <summary>
    /// A token value to validate connections with
    /// </summary>
    public string? BearerToken { get; set; } = BearerToken;
}