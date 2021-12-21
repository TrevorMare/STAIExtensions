using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.Views;

public interface IDataSetView
{

    event EventHandler OnViewUpdated;

    Task OnDataSetUpdated(IDataSet dataset);

}