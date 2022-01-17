﻿using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

/// <summary>
/// CQRS Command to temporarily freeze updates on a View
/// </summary>
public class SetViewDisabledCommand : IRequest<bool>
{

    #region Properties
    /// <summary>
    /// The View Id to freeze updates on
    /// </summary>
    public string ViewId { get; set; }

    /// <summary>
    /// The Owner Id of the View
    /// </summary>
    public string OwnerId { get; set; }

    #endregion
    
    #region ctor

    public SetViewDisabledCommand(string viewId, string ownerId)
    {
        this.ViewId = viewId;
        this.OwnerId = ownerId;
    }
    #endregion
    
}

/// <summary>
/// CQRS Command handler to temporarily freeze updates on a View
/// </summary>
public class SetViewDisabledCommandHandler : IRequestHandler<SetViewDisabledCommand, bool>
{
    
    #region Members
    private readonly IViewCollection _viewCollection;
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public SetViewDisabledCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public Task<bool> Handle(SetViewDisabledCommand request, CancellationToken cancellationToken)
    {
        var result = false;
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        var view = _viewCollection.GetView(request.ViewId, request.OwnerId);
        if (view != null)
        {
            result = view.RefreshEnabled == true;
            view.SetViewRefreshDisabled();
        }

        return Task.FromResult(result);
    }
    #endregion
    
}