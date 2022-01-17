using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;

/// <summary>
/// CQRS Command to un-freeze updates on a View
/// </summary>
public class SetViewEnabledCommand : IRequest<bool>
{

    #region Properties
    /// <summary>
    /// Gets or sets the View Id to un-freeze
    /// </summary>
    public string ViewId { get; set; }

    /// <summary>
    /// Gets or sets the Owner Id of the View
    /// </summary>
    public string OwnerId { get; set; }

    #endregion
    
    #region ctor

    public SetViewEnabledCommand(string viewId, string ownerId)
    {
        this.ViewId = viewId;
        this.OwnerId = ownerId;
    }
    #endregion
    
}

/// <summary>
/// CQRS Command handler to un-freeze updates on a View
/// </summary>
public class SetViewEnabledCommandHandler : IRequestHandler<SetViewEnabledCommand, bool>
{
    
    #region Members
    private readonly IViewCollection _viewCollection;
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public SetViewEnabledCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public Task<bool> Handle(SetViewEnabledCommand request, CancellationToken cancellationToken)
    {
        var result = false;
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        var view = _viewCollection.GetView(request.ViewId, request.OwnerId);
        if (view != null)
        {
            result = view.RefreshEnabled == false;
            view.SetViewRefreshEnabled();
        }

        return Task.FromResult(result);
    }
    #endregion
    
}