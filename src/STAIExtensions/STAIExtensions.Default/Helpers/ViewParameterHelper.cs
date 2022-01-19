using System.Text.Json;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Default.Helpers;

/// <summary>
/// Helper class to extract and convert view parameters
/// </summary>
public static class ViewParameterHelper
{
    /// <summary>
    /// Extracts a parameter from the parameters and converts the object to the type required
    /// </summary>
    /// <param name="viewParameters">The view's parameter values</param>
    /// <param name="key">The parameter name to extract</param>
    /// <param name="telemetryClient">The optional telemetry client to log exceptions on</param>
    /// <param name="logger">The optional logger component to log exceptions on</param>
    /// <typeparam name="T">The expected output type</typeparam>
    /// <returns></returns>
    public static T? ExtractParameter<T>(Dictionary<string, object?>? viewParameters, string key, 
        TelemetryClient? telemetryClient = default, ILogger? logger = default)
    {
        var result = default(T);

        if (viewParameters == null)
            return result;

        if (!viewParameters.ContainsKey(key))
            return result;

        object? parameterValue = viewParameters[key];
        if (parameterValue == null)
            return result;
        
        // Convert the object by using a Json Serialize / De-Serialize method
        try
        {
            var jsonValue = System.Text.Json.JsonSerializer.Serialize(parameterValue);
            
            result = System.Text.Json.JsonSerializer.Deserialize<T>(jsonValue, new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            });

            return result;
        }
        catch (Exception e)
        {
            Abstractions.Common.ErrorLoggingFactory.LogError(telemetryClient, logger, e,
                "An error occured converting the parameter value: {ErrorMessage}", e.Message);
            return result;
        }
    }
    
}