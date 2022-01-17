using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSets.Commands;


/// <summary>
/// CQRS command to detach a DataSet instance from the DataSet Collection instance
/// </summary>
public class DetachDataSetCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the DataSet to detach to the DataSet Collection instance
    /// </summary>
    public IDataSet DataSet { get; set; }

    public DetachDataSetCommand(IDataSet dataSet)
    {
        this.DataSet = dataSet;
    }
}

/// <summary>
/// CQRS command handler to detach a DataSet instance to the DataSet Collection instance
/// </summary>
public class DetachDataSetCommandHandler : IRequestHandler<DetachDataSetCommand, bool>
{
    
    #region Members
    private readonly TelemetryClient? _telemetryClient;
    private readonly IDataSetCollection _dataSetCollection;
    #endregion

    #region ctor
    public DetachDataSetCommandHandler(TelemetryClient? telemetryClient)
    {
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public Task<bool> Handle(DetachDataSetCommand request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        return Task.FromResult(_dataSetCollection.DetachDataSet(request.DataSet));
    }
    #endregion
    
}