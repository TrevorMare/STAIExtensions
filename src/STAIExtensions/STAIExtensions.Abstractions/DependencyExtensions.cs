using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Abstractions;

public static class DependencyExtensions
{

    #region Members

    private static IServiceCollection? _serviceCollection = null;
    private static IServiceProvider? _serviceProvider = null;
    private static ILoggerFactory? _loggerFactory = null;
    #endregion

    #region Properties
    public static IServiceProvider? ServiceProvider
    {
        get { return _serviceProvider ??= _serviceCollection?.BuildServiceProvider(); }
    }

    public static ILoggerFactory? LoggerFactory
    {
        get { return _loggerFactory ??= ServiceProvider?.GetService<ILoggerFactory>(); }
    }
    #endregion

    #region Extension Method
    public static IServiceCollection UseSTAIExtensions(this IServiceCollection serviceCollection)
    {
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