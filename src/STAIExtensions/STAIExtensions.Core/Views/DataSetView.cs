using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Views;

public abstract class DataSetView : Abstractions.Views.IDataSetView
{

    #region Members

    private bool _disposed = false;

    #endregion

    public event EventHandler OnDisposing;
    
    public event EventHandler OnViewUpdated;
    
    public virtual Task OnDataSetUpdated(IDataSet dataset)
    {
        OnViewUpdated?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }

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
 