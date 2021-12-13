namespace STAIExtensions.Abstractions.ApiClient.Models;

public record class WebApiResponse(string? ResponseData, bool Success, string? ErrorMessage = default);