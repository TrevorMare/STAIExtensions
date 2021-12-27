using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Views;

public abstract class DataSetView : Abstractions.Views.IDataSetView
{
    
    #region Members

    private string _viewId = Guid.NewGuid().ToString();
    
    private bool _disposed = false;
    
    private TimeSpan _slidingExpiration = TimeSpan.FromMinutes(5);
    #endregion
    
    #region Properties

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
    #endregion

    #region Events
    public event EventHandler? OnDisposing;
    
    public event EventHandler? OnViewUpdated;
    #endregion

    #region Methods
    public virtual Task OnDataSetUpdated(IDataSet dataset)
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
    #endregion

    #region Dispose

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _disposed == false)
        {
            _disposed = true;
            this.OnDisposing?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion
    
}   
 