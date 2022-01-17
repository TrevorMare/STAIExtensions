using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.Management.Commands;

/// <summary>
/// CQRS Command to remove all views that had no interaction with in the allowed timeframe
/// </summary>
public class ExpireViewsCommand : IRequest<int>
{
    
}

/// <summary>
/// CQRS Command handler to expire all views that had no interaction with in the allowed timeframe
/// </summary>
public class ExpireViewsCommandHandler : IRequestHandler<ExpireViewsCommand, int>
{
    #region Members

    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public ExpireViewsCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods

    public Task<int> Handle(ExpireViewsCommand request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        var expiredViews = _viewCollection.GetExpiredViews();

        var dataSetViews = expiredViews as IDataSetView[] ?? expiredViews.ToArray();
        
        if (!dataSetViews.Any()) return Task.FromResult(0);

        foreach (var expiredView in dataSetViews)
        {
            _dataSetCollection.RemoveViewFromDataSets(expiredView.Id);
            _viewCollection.RemoveView(expiredView);
        }

        return Task.FromResult(dataSetViews.Count());
    }

    #endregion
    
}
