namespace STAIExtensions.Abstractions.WebApi;

public record class WebApiResponse(string? ResponseData, bool Success, string? ErrorMessage = default);