using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Views;

public abstract class DataSetView : Abstractions.Views.IDataSetView
{
    
    #region Members

    private readonly string _viewId = Guid.NewGuid().ToString();
    
    private TimeSpan _slidingExpiration = TimeSpan.FromMinutes(15);

    private bool _disposed = false;
    
    #endregion
    
    #region Properties

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

    #region Methods
    public virtual Task UpdateViewFromDataSet(IDataSet dataset)
    {
        OnViewUpdated?.Invoke(this, EventArgs.Empty);
        LastUpdate = DateTime.Now;
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
 