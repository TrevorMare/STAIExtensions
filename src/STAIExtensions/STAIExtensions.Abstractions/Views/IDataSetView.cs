using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.Views;

public interface IDataSetView : IDisposable
{

    event EventHandler OnDisposing;
    
    event EventHandler OnViewUpdated;

    Task OnDataSetUpdated(IDataSet dataset);

}