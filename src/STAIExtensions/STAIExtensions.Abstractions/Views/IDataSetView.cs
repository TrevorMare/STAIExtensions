using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.Views;

public interface IDataSetView : IDisposable
{

    string Id { get; }

    string? OwnerId { get; set; }

    DateTime? ExpiryDate { get; }
    
    DateTime? LastUpdate { get; }

    IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors { get; }

    TimeSpan SlidingExpiration { get; set; }

    event EventHandler OnDisposing;
    
    event EventHandler OnViewUpdated;

    Task OnDataSetUpdated(IDataSet dataset);

    void SetExpiryDate();

    void SetExpiryDate(DateTime value);

    void SetViewParameters(Dictionary<string, object>? parameters);
}