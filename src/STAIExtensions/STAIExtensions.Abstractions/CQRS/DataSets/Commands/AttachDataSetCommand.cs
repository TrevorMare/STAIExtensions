using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSets.Commands;

public class AttachDataSetCommand : IRequest<bool>
{
    public IDataSet DataSet { get; set; } 

    public AttachDataSetCommand(IDataSet dataSet)
    {
        this.DataSet = dataSet;
    }
}

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