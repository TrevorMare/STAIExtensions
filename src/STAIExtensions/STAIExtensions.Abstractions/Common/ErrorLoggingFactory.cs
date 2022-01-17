using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Abstractions.Common;

/// <summary>
/// Helper class for logging errors
/// </summary>
public static class ErrorLoggingFactory
{

    /// <summary>
    /// Logs an error to the applicable channel with the telemetry client as the first priority and then on the Logger Channel
    /// if it is not null 
    /// </summary>
    /// <param name="telemetryClient">The optional Application Insights Telemetry client</param>
    /// <param name="logger">The optional ILogger</param>
    /// <param name="ex">The exception to log</param>
    /// <param name="message">The error message</param>
    /// <param name="args">The arguments to format the message with</param>
    public static void LogError(TelemetryClient? telemetryClient, ILogger? logger, Exception ex, string? message, params object?[] args)
    {
        if (telemetryClient == null && logger == null) return;

        if (telemetryClient != null)
        {
            telemetryClient.TrackException(ex);
            return;
        }

        logger?.LogError(ex, message, args);
    }
    
    
}