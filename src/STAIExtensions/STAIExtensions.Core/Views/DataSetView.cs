using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Views;

public abstract class DataSetView : Abstractions.Views.IDataSetView
{

    public event EventHandler OnViewUpdated;
    
    public virtual Task OnDataSetUpdated(IDataSet dataset)
    {
        OnViewUpdated?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }
}   
 