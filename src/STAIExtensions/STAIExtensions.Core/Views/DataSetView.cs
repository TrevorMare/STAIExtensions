using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Views;

/// <summary>
/// Abstract implementation of the Data Set View.
/// </summary>
public abstract class DataSetView : Abstractions.Views.IDataSetView
{
    
    #region Members

    protected readonly ILogger<DataSetView>? Logger;
    
    protected readonly TelemetryClient? TelemetryClient;

    private readonly string _viewId = Guid.NewGuid().ToString();
    
    private TimeSpan _slidingExpiration = TimeSpan.FromMinutes(15);

    private bool _disposed = false;
    
    #endregion
    
    #region Properties

    public string ViewTypeName => this.GetType().AssemblyQualifiedName;
    
    public bool RefreshEnabled { get; protected set; } = true;
    
    public virtual IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors { get; } = null;
    
    /// <summary>
    /// Gets the Auto Generated Id of the View
    /// </summary>
    public string Id => _viewId;
    
    /// <summary>
    /// Gets or sets the Owner Id of the View
    /// </summary>
    public string? OwnerId { get; set; }
    
    /// <summary>
    /// Gets the calculated Expiry date of the view
    /// </summary>
    public DateTime? ExpiryDate { get; protected set; }
    
    /// <summary>
    /// Gets the last time the View was updated
    /// </summary>
    public DateTime? LastUpdate { get; protected set; }

    /// <summary>
    /// Gets or sets the Sliding Expiration of the view for expiry
    /// </summary>
    public TimeSpan SlidingExpiration
    {
        get => _slidingExpiration;
        set
        {
            if (value == _slidingExpiration) return;
            this._slidingExpiration = value;
            this.SetExpiryDate();
        }
    }

    /// <summary>
    /// Gets the view parameters that were set via the <see cref="SetViewParameters"/> method
    /// </summary>
    protected Dictionary<string, object>? ViewParameters { get; private set; } = null;
    #endregion

    #region Events
    /// <summary>
    /// Callback event when a view has been updated by DataSets
    /// </summary>
    public event EventHandler? OnViewUpdated;
    #endregion

    #region ctor

    protected DataSetView()
    {
        TelemetryClient = Abstractions.DependencyExtensions.TelemetryClient;
        Logger = Abstractions.DependencyExtensions.CreateLogger<DataSetView>();
    }

    #endregion

    #region Methods
    /// <summary>
    /// Updates the View from a DataSet. When overriding this method, call the base.UpdateViewFromDataSet()
    /// method to continue the standard logic of notifications
    /// </summary>
    /// <param name="dataset">The DataSet to use as a source to update the view from</param>
    /// <returns></returns>
    public virtual Task UpdateViewFromDataSet(IDataSet dataset)
    {
        using var updateViewOperation = this.TelemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(UpdateViewFromDataSet)}");

        try
        {
            if (dataset == null)
                return Task.CompletedTask;

            BuildViewData(dataset);
            
            if (RefreshEnabled)
                OnViewUpdated?.Invoke(this, EventArgs.Empty);
        
            LastUpdate = DateTime.Now;
            
            this.TelemetryClient?.TrackEvent("UpdateViewFromDataSet", 
                new Dictionary<string, string>()
                {
                    { "DataSetId", dataset.DataSetId },
                    { "ViewId", this.Id }
                });
            
        }
        catch (Exception ex)
        {
            if (updateViewOperation != null)
                updateViewOperation.Telemetry.Success = false;

            Abstractions.Common.ErrorLoggingFactory.LogError(this.TelemetryClient, this.Logger, ex,
                "An error occured updating the view: {ErrorMessage}", ex.Message);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Override this method to process the data set information
    /// </summary>
    /// <param name="dataSet">The data set that updated</param>
    /// <returns></returns>
    protected abstract Task BuildViewData(IDataSet dataSet);

    /// <summary>
    /// Calculates the Next expiry time from the sliding expiry time span
    /// </summary>
    public void SetExpiryDate()
    {
        this.ExpiryDate = DateTime.Now.Add(SlidingExpiration);
    }

    /// <summary>
    /// Explicitly set the expiry date of the view
    /// </summary>
    /// <param name="value"></param>
    public void SetExpiryDate(DateTime value)
    {
        this.ExpiryDate = value;
    }

    /// <summary>
    /// Sets the view parameters
    /// </summary>
    /// <param name="parameters"></param>
    public void SetViewParameters(Dictionary<string, object>? parameters)
    {
        this.ViewParameters = parameters;
    }

    /// <summary>
    /// Un-Freezes the view for further updates
    /// </summary>
    public void SetViewRefreshEnabled()
    {
        this.RefreshEnabled = true;
    }

    /// <summary>
    ///  Freezes the view for further updates
    /// </summary>
    public void SetViewRefreshDisabled()
    {
        this.RefreshEnabled = false;
    }

    #endregion

    #region Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _disposed == false)
        {
            _disposed = true;
            Abstractions.DependencyExtensions.Mediator?.Send(new Abstractions.CQRS.DataSetViews.Commands.RemoveViewCommand(this.Id));
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion
    
}   
 