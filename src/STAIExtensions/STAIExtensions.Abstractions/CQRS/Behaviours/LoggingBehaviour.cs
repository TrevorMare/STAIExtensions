using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Abstractions.CQRS.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{

    #region Members

    private readonly TelemetryClient? _telemetryClient;
    private readonly ILogger<TRequest>? _logger;

    #endregion

    #region ctor

    public LoggingBehaviour(TelemetryClient? telemetryClient, ILogger<TRequest>? logger)
    {
        this._logger = logger;
        this._telemetryClient = telemetryClient;
    }

    #endregion

    #region Public Methods
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var requestName = request?.GetType()?.Name ?? "Unknown request";
        var requestInstanceIdentifier = Guid.NewGuid().ToString();

        using var timedOperation = _telemetryClient?.StartOperation<DependencyTelemetry>(requestName);
        try
        {
            this._logger?.LogTrace("Executing request: {OperationName} {InstanceIdentifier}", requestName,
                requestInstanceIdentifier);
            
            TResponse response;

            if (timedOperation != null)
            {
                timedOperation.Telemetry.Data = this.GetRequestPayload(request);
                timedOperation.Telemetry.Properties["InstanceId"] = requestInstanceIdentifier;
                timedOperation.Telemetry.Properties["RequestName"] = requestName;
            }
                
            response = await next();

            return response;
        }
        catch (Exception ex)
        {
            if (timedOperation != null)
                timedOperation.Telemetry.Success = false;
            this._logger?.LogError(ex, "An error occured during request processing: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    

    #endregion

    #region Private Methods

    private string? GetRequestPayload(TRequest request)
    {
        if (request == null) return null;
        try
        {
            return System.Text.Json.JsonSerializer.Serialize(request);
        }
        catch (NotSupportedException e)
        {
            this._logger?.LogWarning("Unable to serialize request operation data");
        }
        return null;
    }
    

    #endregion
    
}