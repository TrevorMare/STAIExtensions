namespace STAIExtensions.Host.Grpc;

public record class GrpcHostOptions(bool UseDefaultAuthorization = true, string? BearerToken = "598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666")
{
    public bool UseDefaultAuthorization { get; set; } = UseDefaultAuthorization;
    public string? BearerToken { get; set; } = BearerToken;
}