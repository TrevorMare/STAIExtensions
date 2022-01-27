using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.CQRS.Management.Commands;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

/// <summary>
/// Implementation of the Data Set collection. This collection manages all the Data Set instances
/// created at runtime and also notifies linked views of updates and changes to data sets. This collection
/// is further used to in notifying the user of any changes 
/// </summary>
public class DataSetCollection : Abstractions.Collections.IDataSetCollection
{

    #region Events

    /// <summary>
    /// An event that will be raised once a data set has been updated
    /// </summary>
    public event IDataSetCollection.OnDataSetUpdatedHandler? OnDataSetUpdated;

    #endregion
    
    #region Members
    private readonly List<IDataSet> _dataSetCollection = new();
    private readonly Dictionary<string, List<string>> _dataSetAttachedViews = new();
    private readonly ILogger<DataSetCollection>? _logger;
    private readonly DataSetCollectionOptions _options;
    #endregion

    #region ctor
    public DataSetCollection(DataSetCollectionOptions? options)
    {
        _logger = DependencyExtensions.CreateLogger<DataSetCollection>();
        _options = options ?? new DataSetCollectionOptions();
        
        _logger?.LogTrace("Instantiating the data set collection");
    }
    #endregion

    #region Properties

    /// <summary>
    /// The current number of registered DataSets
    /// </summary>
    public int NumberOfDataSets => _dataSetCollection.Count;
    
    /// <summary>
    /// Exposes the maximum number of allowed DataSets from the options
    /// </summary>
    public int? MaximumDataSets => _options.MaximumDataSets;
    
    /// <summary>
    /// Exposes the maximum number of views allowed per DataSet from the options
    /// </summary>
    public int? MaximumViewsPerDataSet => _options.MaximumViewsPerDataSet;

    #endregion
    
    #region Methods

    /// <summary>
    /// Attaches a DataSet to the collection and attaches to the DataSet events
    /// </summary>
    /// <param name="dataSet">The DataSet to attach to the collection</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    public bool AttachDataSet(IDataSet dataSet)
    {
        try
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (_dataSetCollection.Contains(dataSet))
            {
                _logger?.LogInformation("Data set with Id {Id} is already part of the collection", dataSet.DataSetId);
                return false;
            }

            if (MaximumDataSets.HasValue && _dataSetCollection.Count >= MaximumDataSets.Value)
            {
                throw new Exception($"The maximum number ({MaximumDataSets.Value}) of allowed datasets is already attached");
            }

            dataSet.OnDataSetUpdated += OnDataSetObjectUpdated;

            lock (_dataSetAttachedViews)
            {
                _dataSetAttachedViews[dataSet.DataSetId] = new List<string>();
            }

            lock (_dataSetCollection)
            {
                _dataSetCollection.Add(dataSet);
            }

            _logger?.LogInformation("Data set with Id {Id} attached to collection", dataSet.DataSetId);
        
            return true;
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "An error occured attaching the dataset. {Error}", e);
            throw;
        }
    }

    /// <summary>
    /// Detaches a DataSet from the collection and removes all view links from this DataSet. 
    /// </summary>
    /// <param name="dataSet">The DataSet to detach from the collection</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool DetachDataSet(IDataSet dataSet)
    {
        try
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (!_dataSetCollection.Contains(dataSet))
            {
                _logger?.LogInformation(
                    "Data set with Id {Id} could not be detached as it is not part of the collection",
                    dataSet.DataSetId);
                return false;
            }

            dataSet.OnDataSetUpdated -= OnDataSetObjectUpdated;
            dataSet.StopAutoRefresh();

            lock (_dataSetAttachedViews)
            {
                _dataSetAttachedViews.Remove(dataSet.DataSetId);
            }

            lock (_dataSetCollection)
            {
                _dataSetCollection.Remove(dataSet);
            }
        
            _logger?.LogInformation("Data set with Id {Id} detached from collection", dataSet.DataSetId);
            
            return true;
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "An error occured detaching the dataset. {Error}", e);
            throw;
        }
    }

    /// <summary>
    /// Gets a list of attached DataSets from the collection
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DataSetInformation> ListDataSets()
    {
        return _dataSetCollection.Select(dataSet => new DataSetInformation(dataSet.DataSetName, dataSet.DataSetId,
            dataSet.DataSetType, dataSet.FriendlyDataSetType));
    }

    /// <summary>
    /// Attaches a View Instance to a DataSet for updates
    /// </summary>
    /// <param name="dataSetId">The unique Id of the DataSet</param>
    /// <param name="view">The view to link for updates</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    public bool AttachViewToDataSet(string dataSetId, IDataSetView view)
    {
        try
        {
            if (string.IsNullOrEmpty(dataSetId) || dataSetId.Trim() == "")
                throw new ArgumentNullException(nameof(dataSetId));
            
            if (view == null)
                throw new ArgumentNullException(nameof(view));
        
            var dataSet = this.FindDataSetById(dataSetId);
            if (dataSet == null)
            {
                _logger?.LogWarning(
                    "Data set with Id {DataSetId} could not be located to attach the view with Id {ViewId} to",
                    dataSetId, view.Id);
                return false;
            }

            lock (_dataSetAttachedViews)
            {
                if (!_dataSetAttachedViews[dataSetId].Contains(view.Id.ToLower()))
                {

                    if (MaximumViewsPerDataSet.HasValue &&
                        _dataSetAttachedViews[dataSetId].Count >= MaximumViewsPerDataSet.Value)
                        throw new Exception(
                            $"The dataset with Id {dataSetId} already has the maximum number ({MaximumViewsPerDataSet.Value} of allowed views attached)");
                
                    _dataSetAttachedViews[dataSetId].Add(view.Id.ToLower());
                    _logger?.LogInformation(
                        "View with Id {ViewId} is now attached to data set {DataSetId}",
                        view.Id, dataSet.DataSetId);
                    return true;
                }
            }
        
            _logger?.LogInformation(
                "View with Id {ViewId} is already attached to data set {DataSetId}",
                view.Id, dataSet.DataSetId);
        
            return false;
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "An error occured attaching view the dataset. {Error}", e);
            throw;
        }
    }

    /// <summary>
    /// Detaches a View Instance from a DataSet to remove updates
    /// </summary>
    /// <param name="dataSetId">The unique Id of the DataSet</param>
    /// <param name="view">The view to link for updates</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool DetachViewFromDataSet(string dataSetId, IDataSetView view)
    {
        try
        {
            if (string.IsNullOrEmpty(dataSetId) || dataSetId.Trim() == "")
                throw new ArgumentNullException(nameof(dataSetId));
               
            if (view == null)
                throw new ArgumentNullException(nameof(view));
        
            var dataSet = this.FindDataSetById(dataSetId);
            if (dataSet == null)
            {
                _logger?.LogWarning(
                    "Data set with Id {DataSetId} could not be located to detach the view with Id {ViewId} to",
                    dataSetId, view.Id);
                return false;
            }

            lock (_dataSetAttachedViews)
            {
                if (_dataSetAttachedViews[dataSetId].Contains(view.Id.ToLower()))
                {
                    _dataSetAttachedViews[dataSetId].Remove(view.Id.ToLower());
                    _logger?.LogInformation(
                        "View with Id {ViewId} is now detached from data set {DataSetId}",
                        view.Id, dataSet.DataSetId);
                    return true;
                }
            }

            _logger?.LogInformation(
                "View with Id {ViewId} is not attached to data set {DataSetId}",
                view.Id, dataSet.DataSetId);

            return false;
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "An error occured attaching view the dataset. {Error}", e);
            throw;
        }
     
    }

    /// <summary>
    /// Finds an attached DataSet by Id
    /// </summary>
    /// <param name="dataSetId">The unique Id of the DataSet</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public IDataSet? FindDataSetById(string dataSetId)
    {
        try
        {
            if (string.IsNullOrEmpty(dataSetId) || dataSetId.Trim() == "")
                throw new ArgumentNullException(nameof(dataSetId));

            return this._dataSetCollection.FirstOrDefault(ds =>
                string.Equals(ds.DataSetId.Trim(), dataSetId.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Could not locate the data set. {Error}", e);
            throw;
        }
    }

    /// <summary>
    /// Finds an attached DataSet by name
    /// </summary>
    /// <param name="dataSetName">The unique name of the dataset</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public IDataSet? FindDataSetByName(string dataSetName)
    {
        try
        {
            if (string.IsNullOrEmpty(dataSetName) || dataSetName.Trim() == "")
                throw new ArgumentNullException(nameof(dataSetName));

            return this._dataSetCollection.FirstOrDefault(ds =>
                string.Equals(ds.DataSetName.Trim(), dataSetName.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Could not locate the data set. {Error}", e);
            throw;
        }
    }

    /// <summary>
    /// Finds all the DataSets that are linked to the view
    /// </summary>
    /// <param name="viewId">The unique Id of the view to find the links with</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public IEnumerable<IDataSet>? FindDataSetsByViewId(string viewId)
    {
        
        try
        {
            if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
                throw new ArgumentNullException(nameof(viewId));

            lock (_dataSetAttachedViews)
            {
                var dataSetIdsAttachedToView =
                    (from q in _dataSetAttachedViews
                        where q.Value.Any(x => string.Equals(x.Trim(), viewId.Trim(), StringComparison.OrdinalIgnoreCase))
                        select q.Key.ToLower()).Distinct().ToList();
                
                return this._dataSetCollection.Where(ds => dataSetIdsAttachedToView.Contains(ds.DataSetId.ToLower().Trim()));
            }
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Could not locate the data set. {Error}", e);
            throw;
        }
        
    }
    
    /// <summary>
    /// Removes a View link from all DataSets in the collection
    /// </summary>
    /// <param name="viewId">The view Id to find and remove from updates</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void RemoveViewFromDataSets(string viewId)
    {

        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        _logger?.LogWarning("Removing view with Id {Id} from all datasets", viewId);
        
        lock (_dataSetAttachedViews)
        {
            _dataSetAttachedViews.Keys.ToList().ForEach(key =>
            {
                _dataSetAttachedViews[key].RemoveAll(x => string.Equals(x.Trim(), viewId.Trim(), StringComparison.OrdinalIgnoreCase));
            });
        }
    }
    #endregion

    #region Private Methods

    private async Task RunCleanup()
    {
        await Abstractions.DependencyExtensions.Mediator?.Send(
            new ExpireViewsCommand())!;
    }

    private async void OnDataSetObjectUpdated(object? sender, EventArgs e)
    {
        try
        {
            if (sender is not IDataSet dataSet)
                return;

            _logger?.LogInformation("Data set with Id {Id} updated", dataSet.DataSetId);

            OnDataSetUpdated?.Invoke(dataSet, EventArgs.Empty);

            List<string> viewIds;
            lock (_dataSetAttachedViews)
            {
                viewIds = _dataSetAttachedViews[dataSet.DataSetId].ToList();    
            }
            
            foreach (var viewId in viewIds)
            {
                _logger?.LogInformation("Updating view {ViewId} with data set Id {DataSetId}", viewId,
                    dataSet.DataSetId);
                await DependencyExtensions.Mediator?.Send(
                    new Abstractions.CQRS.DataSetViews.Commands.UpdateViewFromDataSetCommand(viewId, dataSet))!;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "An error occured updating the dataset views");
        }
        finally
        {
            await this.RunCleanup();
        }
    }
    #endregion
    
}