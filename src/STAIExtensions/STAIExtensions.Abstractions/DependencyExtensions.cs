using System.Reflection;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.Collections;

namespace STAIExtensions.Abstractions;

/// <summary>
/// Static class to register the required services in STAIExtensions
/// </summary>
public static class DependencyExtensions
{

    #region Members
    private static IServiceCollection? _serviceCollection = null;
    private static IServiceProvider? _serviceProvider = null;
    private static ILoggerFactory? _loggerFactory = null;
    private static IMediator? _mediator = null;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the static service provider object
    /// </summary>
    public static IServiceProvider? ServiceProvider
    {
        get { return _serviceProvider ??= _serviceCollection?.BuildServiceProvider(); }
    }

    /// <summary>
    /// Gets the static logger factory registered with the service provider
    /// </summary>
    public static ILoggerFactory? LoggerFactory
    {
        get { return _loggerFactory ??= ServiceProvider?.GetService<ILoggerFactory>(); }
    }
    
    /// <summary>
    /// Gets the static MediatR object registered with the service provider
    /// </summary>
    public static IMediator? Mediator
    {
        get { return _mediator ??= ServiceProvider?.GetService<IMediator>(); }
    }

    /// <summary>
    /// Gets the Telemetry client object registered with the service provider
    /// </summary>
    public static TelemetryClient? TelemetryClient =>
        (TelemetryClient?) ServiceProvider?.GetService(typeof(TelemetryClient));
    #endregion

    #region Extension Method
    /// <summary>
    /// Registers the required services used by STAIExtensions
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection UseSTAIExtensionsAbstractions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(CQRS.Behaviours.LoggingBehaviour<,>));
        
        _serviceCollection ??= serviceCollection;

        return serviceCollection;
    }
    #endregion

    #region Methods

    public static ILogger<T>? CreateLogger<T>()
    {
        return LoggerFactory?.CreateLogger<T>();
    }
    #endregion
    
}