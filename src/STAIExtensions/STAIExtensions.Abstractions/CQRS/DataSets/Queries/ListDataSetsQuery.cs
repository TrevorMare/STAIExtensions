using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Abstractions.CQRS.DataSets.Queries;

public class ListDataSetsQuery : IRequest<IEnumerable<DataSetInformation>>
{
   
}

public class ListDataSetsQueryHandler : IRequestHandler<ListDataSetsQuery, IEnumerable<DataSetInformation>>
{

    #region Members

    private readonly IDataSetCollection _dataSetCollection;
    private readonly TelemetryClient? _telemetryClient;

    #endregion
    
    #region ctor

    public ListDataSetsQueryHandler()
    {
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public Task<IEnumerable<DataSetInformation>> Handle(ListDataSetsQuery request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        return Task.FromResult(this._dataSetCollection.ListDataSets());
    }
    #endregion
    
}