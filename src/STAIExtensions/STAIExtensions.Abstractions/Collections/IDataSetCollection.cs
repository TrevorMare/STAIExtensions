using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Collections;

/// <summary>
/// Interface component that will keep track of the all the registered DataSets
/// and Register/DeRegister the DataSets when required. It also keeps track
/// of user instance views that are linked to a DataSet 
/// </summary>
public interface IDataSetCollection
{

    /// <summary>
    /// Delegate for the OnDataSetUpdated event <see cref="OnDataSetUpdated"/>
    /// </summary>
    delegate void OnDataSetUpdatedHandler(IDataSet sender, EventArgs e);

    /// <summary>
    /// Exposes the maximum number of allowed DataSets from the options
    /// </summary>
    int? MaximumDataSets { get; }

    /// <summary>
    /// Exposes the maximum number of views allowed per DataSet from the options
    /// </summary>
    int? MaximumViewsPerDataSet { get; }

    /// <summary>
    /// The current number of registered DataSets
    /// </summary>
    int NumberOfDataSets { get; }

    /// <summary>
    /// An event that gets raised when a DataSet is updated
    /// </summary>
    event OnDataSetUpdatedHandler OnDataSetUpdated;
    
    /// <summary>
    /// Attaches a DataSet to the collection and attaches to the DataSet events
    /// </summary>
    /// <param name="dataSet">The DataSet to attach to the collection</param>
    /// <returns></returns>
    bool AttachDataSet(IDataSet dataSet);
    
    /// <summary>
    /// Detaches a DataSet from the collection and removes all view links from this DataSet. 
    /// </summary>
    /// <param name="dataSet">The DataSet to detach from the collection</param>
    /// <returns></returns>
    bool DetachDataSet(IDataSet dataSet);

    /// <summary>
    /// Gets a list of attached DataSets from the collection
    /// </summary>
    /// <returns></returns>
    IEnumerable<DataSetInformation> ListDataSets();

    /// <summary>
    /// Attaches a View Instance to a DataSet for updates
    /// </summary>
    /// <param name="dataSetId">The unique Id of the DataSet</param>
    /// <param name="view">The view to link for updates</param>
    /// <returns></returns>
    bool AttachViewToDataSet(string dataSetId, IDataSetView view);
    
    /// <summary>
    /// Detaches a View Instance from a DataSet to remove updates
    /// </summary>
    /// <param name="dataSetId">The unique Id of the DataSet</param>
    /// <param name="view">The view to link for updates</param>
    /// <returns></returns>
    bool DetachViewFromDataSet(string dataSetId, IDataSetView view);

    /// <summary>
    /// Finds an attached DataSet by Id
    /// </summary>
    /// <param name="dataSetId">The unique Id of the DataSet</param>
    /// <returns></returns>
    IDataSet? FindDataSetById(string dataSetId);
    
    /// <summary>
    /// Finds an attached DataSet by name
    /// </summary>
    /// <param name="datasetName">The unique name of the dataset</param>
    /// <returns></returns>
    IDataSet? FindDataSetByName(string datasetName);
    
    /// <summary>
    /// Removes a View link from all DataSets in the collection
    /// </summary>
    /// <param name="expiredViewId">The view Id to find and remove from updates</param>
    void RemoveViewFromDataSets(string expiredViewId);
 
    /// <summary>
    /// Finds all the DataSets that are linked to the view
    /// </summary>
    /// <param name="requestViewId">The unique Id of the view to find the links with</param>
    /// <returns></returns>
    IEnumerable<IDataSet>? FindDataSetsByViewId(string requestViewId);
}