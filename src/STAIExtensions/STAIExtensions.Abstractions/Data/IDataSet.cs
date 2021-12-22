using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Abstractions.Data;

public interface IDataSet 
{
    
    List<Views.IDataSetView> Views { get; }
    
    string DataSetName { get; set; }

    event EventHandler? OnDataSetUpdated;
    
    bool AutoRefreshEnabled { get; }

    void StartAutoRefresh(TimeSpan autoRefreshInterval, CancellationToken? cancellationToken = default);

    void StopAutoRefresh();
    
    Task UpdateDataSet();

    void RegisterView(Views.IDataSetView datasetView);
    
    void DeRegisterView(Views.IDataSetView datasetView);

}