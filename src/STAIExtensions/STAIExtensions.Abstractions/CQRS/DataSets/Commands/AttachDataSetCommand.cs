using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSets.Commands;

/// <summary>
/// CQRS command to attach a DataSet instance to the DataSet Collection instance
/// </summary>
public class AttachDataSetCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the DataSet to attach to the DataSet Collection instance
    /// </summary>
    public IDataSet DataSet { get; set; } 
    
    public AttachDataSetCommand(IDataSet dataSet)
    {
        this.DataSet = dataSet;
    }
}

/// <summary>
/// CQRS command handler to attach a DataSet instance to the DataSet Collection instance
/// </summary>
public class AttachDataSetCommandHandler : IRequestHandler<AttachDataSetCommand, bool>
{
    
    #region Members
    private readonly TelemetryClient? _telemetryClient;
    private readonly IDataSetCollection _dataSetCollection;
    #endregion

    #region ctor
    public AttachDataSetCommandHandler()
    {
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public Task<bool> Handle(AttachDataSetCommand request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        return Task.FromResult(_dataSetCollection.AttachDataSet(request.DataSet));
    }
    #endregion
    
}