using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

/// <summary>
/// CQRS Command to remove the link from the view to the DataSet
/// </summary>
public class DetachViewFromDataSetCommand : IRequest<bool>
{
    #region Properties

    /// <summary>
    /// Gets or sets the View Id to find for the operation
    /// </summary>
    public string ViewId { get; set; } = "";

    /// <summary>
    /// Gets or sets the Data Set Id to remove the link from
    /// </summary>
    public string DataSetId { get; set; } = "";

    /// <summary>
    /// Gets or sets the Owner Id of the View
    /// </summary>
    public string OwnerId { get; set; } = "";

    #endregion

    #region ctor

    public DetachViewFromDataSetCommand()
    {
        
    }

    public DetachViewFromDataSetCommand(string viewId, string dataSetId, string ownerId)
    {
        this.ViewId = viewId;
        this.DataSetId = dataSetId;
        this.OwnerId = ownerId;
    }
    #endregion
}

/// <summary>
/// CQRS Command handler to remove the link from the view to the DataSet
/// </summary>
public class DetachViewFromDataSetCommandHandler : IRequestHandler<DetachViewFromDataSetCommand, bool>
{
    
    #region Members

    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;
    private readonly TelemetryClient? _telemetryClient;
    
    #endregion

    #region ctor
    public DetachViewFromDataSetCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public Task<bool> Handle(DetachViewFromDataSetCommand request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        var view = _viewCollection.GetView(request.ViewId, request.OwnerId);

        return Task.FromResult(view != null && _dataSetCollection.DetachViewFromDataSet(request.DataSetId, view));
    }
    #endregion
}
