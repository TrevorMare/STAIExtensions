using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

/// <summary>
/// CQRS command to register a view for updates on a DataSet
/// </summary>
public class AttachViewToDataSetCommand : IRequest<bool>
{
    
    #region Properties
    /// <summary>
    /// Gets or sets the View Id that was previously created
    /// </summary>
    public string ViewId { get; set; } 

    /// <summary>
    /// The Data Set Id to attach the view to
    /// </summary>
    public string DataSetId { get; set; } 

    /// <summary>
    /// The owner Id of the view
    /// </summary>
    public string OwnerId { get; set; }

    #endregion

    #region ctor

    public AttachViewToDataSetCommand(string viewId, string dataSetId, string ownerId)
    {
        this.ViewId = viewId;
        this.DataSetId = dataSetId;
        this.OwnerId = ownerId;
    }
    #endregion
}

/// <summary>
/// CQRS command handler to register a view for updates on a DataSet
/// </summary>
public class AttachViewToDataSetCommandHandler : IRequestHandler<AttachViewToDataSetCommand, bool>
{

    #region Members
    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public AttachViewToDataSetCommandHandler(TelemetryClient? telemetryClient)
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public Task<bool> Handle(AttachViewToDataSetCommand request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        var view = _viewCollection.GetView(request.ViewId, request.OwnerId);
        return Task.FromResult(view != null && _dataSetCollection.AttachViewToDataSet(request.DataSetId, view));
    }
    #endregion

   
}