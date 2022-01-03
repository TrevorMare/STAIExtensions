namespace STAIExtensions.Data.AzureDataExplorer.Models;

internal record WebApiResponse(string? ResponseData, bool Success, string? ErrorMessage = default)
{
    public string? ResponseData { get; } = ResponseData;
    public bool Success { get; } = Success;
    public string? ErrorMessage { get; } = ErrorMessage;
}