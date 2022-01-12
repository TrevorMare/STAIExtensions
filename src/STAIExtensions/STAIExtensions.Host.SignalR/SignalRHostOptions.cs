namespace STAIExtensions.Host.SignalR;

public record SignalRHostOptions(bool UseDefaultAuthorization = true, string? BearerToken = "1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495")
{
    public bool UseDefaultAuthorization { get; set; } = UseDefaultAuthorization;
    public string? BearerToken { get; set; } = BearerToken;
}
