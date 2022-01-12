using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;

public class GetDataViewQuery : IRequest<IDataSetView?>
{

    public string ViewId { get; set; } = "";

    public string OwnerId { get; set; } = "";

    #region ctor

    public GetDataViewQuery() {}

    public GetDataViewQuery(string viewId, string ownerId)
    {
        this.ViewId = viewId;
        this.OwnerId = ownerId;
    }
    #endregion
    
}

public class GetDataViewQueryHandler : IRequestHandler<GetDataViewQuery, IDataSetView?>
{

    #region Members

    private readonly IViewCollection _viewCollection;
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public GetDataViewQueryHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public Task<IDataSetView?> Handle(GetDataViewQuery request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        return Task.FromResult(_viewCollection.GetView(request.ViewId, request.OwnerId ));
    }
    #endregion
    
}