using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;

public class SetViewEnabledCommand : IRequest<bool>
{

    #region Properties

    public string ViewId { get; set; }

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