using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Abstractions.Data;

public interface IDataSet : IDisposable
{
    
    string DataSetName { get; set; }

    string DataSetId { get; set; }
    
    string DataSetType { get; }

    event EventHandler? OnDataSetUpdated;
    
    bool AutoRefreshEnabled { get; }

    void StartAutoRefresh(TimeSpan autoRefreshInterval, CancellationToken? cancellationToken = default);

    void StopAutoRefresh();
    
    Task UpdateDataSet();

}