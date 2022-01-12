using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;

public class GetMyViewsQuery : IRequest<IEnumerable<MyViewInformation>>
{

    #region Properties

    public string OwnerId { get; set; }

    #endregion

    public GetMyViewsQuery(string ownerId)
    {
        this.OwnerId = ownerId;
    }
}

public class GetMyViewsQueryHandler : IRequestHandler<GetMyViewsQuery, IEnumerable<MyViewInformation>>
{
    
    #region Members

    private readonly IViewCollection _viewCollection;
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public GetMyViewsQueryHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion
    
    public Task<IEnumerable<MyViewInformation>> Handle(GetMyViewsQuery request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        return Task.FromResult(_viewCollection.GetMyViews(request.OwnerId ));
    }
}
