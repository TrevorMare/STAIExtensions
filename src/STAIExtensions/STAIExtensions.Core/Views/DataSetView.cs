using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Views;

public abstract class DataSetView : Abstractions.Views.IDataSetView
{
    
    #region Members

    protected readonly ILogger<DataSetView>? Logger;
    
    private readonly TelemetryClient? _telemetryClient;

    private readonly string _viewId = Guid.NewGuid().ToString();
    
    private TimeSpan _slidingExpiration = TimeSpan.FromMinutes(15);

    private bool _disposed = false;
    
    #endregion
    
    #region Properties

    public string ViewTypeName => this.GetType().AssemblyQualifiedName;
    
    public bool RefreshEnabled { get; protected set; } = true;
    
    public virtual IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors { get; } = null;
    
    public string Id => _viewId;
    
    public string? OwnerId { get; set; }
    
    public DateTime? ExpiryDate { get; protected set; }
    
    public DateTime? LastUpdate { get; protected set; }

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

    protected Dictionary<string, object>? ViewParameters { get; private set; } = null;
    #endregion

    #region Events
    public event EventHandler? OnViewUpdated;
    #endregion

    #region ctor

    protected DataSetView()
    {
        _telemetryClient =
            (TelemetryClient?) Abstractions.DependencyExtensions.ServiceProvider?.GetService(typeof(TelemetryClient));
        Logger = Abstractions.DependencyExtensions.CreateLogger<DataSetView>();
    }

    #endregion

    #region Methods
    public virtual Task UpdateViewFromDataSet(IDataSet dataset)
    {
        using var updateViewOperation = this._telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(UpdateViewFromDataSet)}");

        try
        {
            if (dataset == null)
                return Task.CompletedTask;
        
            if (RefreshEnabled)
                OnViewUpdated?.Invoke(this, EventArgs.Empty);
        
            LastUpdate = DateTime.Now;
            
            this._telemetryClient?.TrackEvent("UpdateViewFromDataSet", 
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
            
            this.Logger?.LogError(ex, "An error occured updating the view: {ErrorMessage}", ex.Message);
        }
        return Task.CompletedTask;
    }

    public void SetExpiryDate()
    {
        this.ExpiryDate = DateTime.Now.Add(SlidingExpiration);
    }

    public void SetExpiryDate(DateTime value)
    {
        this.ExpiryDate = value;
    }

    public void SetViewParameters(Dictionary<string, object>? parameters)
    {
        this.ViewParameters = parameters;
    }

    public void SetViewRefreshEnabled()
    {
        this.RefreshEnabled = true;
    }

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
 