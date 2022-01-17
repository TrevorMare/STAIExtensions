using MediatR;
using MediatR.Pipeline;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

/// <summary>
/// CQRS Command to set the parameters on view
/// </summary>
public class SetViewParametersCommand : IRequest<bool>
{

    #region Properties
    /// <summary>
    /// Gets or sets the View Id to set the parameters on
    /// </summary>
    public string ViewId { get; set; }

    /// <summary>
    /// Gets or sets the owner of the view
    /// </summary>
    public string OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the parameter values to set. This can be null to reset the view.
    /// </summary>
    public Dictionary<string, object>? ViewParameters { get; set; }

    #endregion

    #region ctor

    public SetViewParametersCommand(string viewId, string ownerId, Dictionary<string, object>? viewParameters)
    {
        this.ViewId = viewId;
        this.OwnerId = ownerId;
        this.ViewParameters = viewParameters;
    }
    #endregion
    
}

/// <summary>
/// CQRS Command handler to set the parameters on view. After the parameters are
/// set, the view will automatically update from the last instances of the attached DataSets
/// </summary>
public class SetViewParametersCommandHandler : IRequestHandler<SetViewParametersCommand, bool>
{
   
    #region Members
    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public SetViewParametersCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion
    
    public async Task<bool> Handle(SetViewParametersCommand request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        var view = _viewCollection.GetView(request.ViewId, request.OwnerId);

        if (view == null) return false;
        view.SetViewParameters(request.ViewParameters);
        
        var dataSets = _dataSetCollection.FindDataSetsByViewId(request.ViewId);
        if (dataSets == null || dataSets.Any() == false) return true;

        var enumerable = dataSets as IDataSet[] ?? dataSets.ToArray();
        foreach (var dataSet in enumerable)
        {
            if (view.RefreshEnabled)
                await view.UpdateViewFromDataSet(dataSet)!;    
            else
                view.SetExpiryDate();
        }
        
        return true;
    }
}