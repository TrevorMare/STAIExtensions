namespace STAIExtensions.Data.AzureDataExplorer.Models;

internal record WebApiResponse(string? ResponseData, bool Success, string? ErrorMessage = default);