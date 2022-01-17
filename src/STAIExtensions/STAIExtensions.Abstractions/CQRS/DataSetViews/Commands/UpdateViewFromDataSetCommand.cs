using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

/// <summary>
/// CQRS Command to update a View from a DataSet
/// </summary>
public class UpdateViewFromDataSetCommand : IRequest<bool>
{
    
    /// <summary>
    /// Gets or sets the View Id to update
    /// </summary>
    public string ViewId { get; set; }

    /// <summary>
    /// Gets or sets the DataSet source to update the view with
    /// </summary>
    public IDataSet DataSet { get; set; }

    public UpdateViewFromDataSetCommand(string viewId, IDataSet dataSet)
    {
        this.ViewId = viewId;
        this.DataSet = dataSet;
    }
}

/// <summary>
/// CQRS Command handler to update a View from a DataSet
/// </summary>
public class UpdateViewFromDataSetCommandHandler : IRequestHandler<UpdateViewFromDataSetCommand, bool>
{
    
    #region Members
    private readonly IViewCollection _viewCollection;
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public UpdateViewFromDataSetCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public async Task<bool> Handle(UpdateViewFromDataSetCommand request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        var view = _viewCollection.GetViewForUpdate(request.ViewId);
        if (view == null) return false;
        
        if (view.RefreshEnabled)
            await view.UpdateViewFromDataSet(request.DataSet)!;
        else
            view.SetExpiryDate();
        return true;
    }
    #endregion
}
