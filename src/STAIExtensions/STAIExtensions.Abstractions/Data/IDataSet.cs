using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Abstractions.Data;

/// <summary>
/// Interface that defines the basic DataSet structure
/// </summary>
public interface IDataSet : IDisposable
{
    
    /// <summary>
    /// Gets or sets the Data Set Friendly name
    /// </summary>
    string DataSetName { get; set; }

    /// <summary>
    /// Gets or sets the unique Id of the DataSet
    /// </summary>
    string DataSetId { get; set; }
    
    /// <summary>
    /// Gets the fully qualified type name of the DataSet
    /// </summary>
    string DataSetType { get; }

    /// <summary>
    /// An Event that occurs once all queries has run and the dataset has updated
    /// </summary>
    event EventHandler? OnDataSetUpdated;
    
    /// <summary>
    /// Gets a value indicating if the DataSet is currently Auto-Refreshing
    /// </summary>
    bool AutoRefreshEnabled { get; }

    /// <summary>
    /// Starts the Auto Refresh process
    /// </summary>
    /// <param name="autoRefreshInterval">The Auto Refresh Interval</param>
    /// <param name="cancellationToken">A Cancellation Token to stop the processing with</param>
    void StartAutoRefresh(TimeSpan autoRefreshInterval, CancellationToken? cancellationToken = default);

    /// <summary>
    /// Stops the DataSet Auto-Refresh processing
    /// </summary>
    void StopAutoRefresh();
    
    /// <summary>
    /// A method that will be executed on the internal auto refresh interval
    /// </summary>
    /// <returns></returns>
    Task UpdateDataSet();

}