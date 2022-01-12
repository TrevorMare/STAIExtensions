using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Abstractions.Common;

public static class ErrorLoggingFactory
{

    public static void LogError(TelemetryClient? telemetryClient, ILogger? logger, Exception ex, string? message, params object?[] args)
    {
        if (telemetryClient == null && logger == null) return;

        if (telemetryClient != null)
        {
            telemetryClient.TrackException(ex);
            return;
        }

        logger.LogError(ex, message, args);
    }
    
    
}