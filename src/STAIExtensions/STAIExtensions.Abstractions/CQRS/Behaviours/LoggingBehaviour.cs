using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Abstractions.CQRS.Behaviours;

/// <summary>
/// A pipeline MediatR behaviour that logs information and telemetry on requests and responses.
/// By default this will track exceptions and Dependency Telemetry and Timings
/// </summary>
/// <typeparam name="TRequest">The MediatR request</typeparam>
/// <typeparam name="TResponse">The MediatR response</typeparam>
public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{

    #region Members

    private readonly TelemetryClient? _telemetryClient;
    private readonly ILogger<TRequest>? _logger;

    #endregion

    #region ctor

    public LoggingBehaviour()
    {
        this._logger = DependencyExtensions.CreateLogger<TRequest>();
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }

    #endregion

    #region Public Methods
    
    /// <summary>
    /// Method to handle and log the information of the request and the response.
    /// </summary>
    /// <param name="request">The MediatR request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="next">The next Request handler delegate</param>
    /// <returns></returns>
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

            Common.ErrorLoggingFactory.LogError(_telemetryClient, _logger, ex,
                "An error occured during request processing: {ErrorMessage}", ex.Message);

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