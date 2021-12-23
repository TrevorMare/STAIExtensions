﻿using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.Collections;

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
    
    public static IViewCollection? ViewCollection => ServiceProvider?.GetService<IViewCollection>();

    public static IDataSetCollection? DataSetCollection => ServiceProvider?.GetService<IDataSetCollection>();
    #endregion

    #region Extension Method
    public static IServiceCollection UseSTAIExtensions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
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