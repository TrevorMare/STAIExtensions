using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.CQRS.Management.Commands;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

public class DataSetCollection : Abstractions.Collections.IDataSetCollection
{

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

    #region Methods
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

            if (_options.MaximumDataSets.HasValue && _dataSetCollection.Count >= _options.MaximumDataSets.Value)
            {
                throw new Exception($"The maximum number ({_options.MaximumDataSets.Value}) of allowed datasets is already attached");
            }

            dataSet.OnDataSetUpdated += OnDataSetUpdated;

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

            dataSet.OnDataSetUpdated -= OnDataSetUpdated;
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

    public IEnumerable<DataSetInformation> ListDataSets()
    {
        return _dataSetCollection.Select(dataSet => new DataSetInformation(dataSet.DataSetName, dataSet.DataSetId, dataSet.DataSetType));
    }

    public bool AttachViewToDataSet(string dataSetId, IDataSetView view)
    {
        try
        {
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

                    if (_options.MaximumViewsPerDataSet.HasValue &&
                        _dataSetAttachedViews[dataSetId].Count >= _options.MaximumViewsPerDataSet.Value)
                        throw new Exception(
                            $"The dataset with Id {dataSetId} already has the maximum number ({_options.MaximumViewsPerDataSet.Value} of allowed views attached)");
                
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

    public bool DetachViewFromDataSet(string dataSetId, IDataSetView view)
    {
        try
        {
               
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

    public IDataSet? FindDataSetById(string dataSetId)
    {
        try
        {
            if (string.IsNullOrEmpty(dataSetId))
                throw new ArgumentNullException(nameof(dataSetId));
        
            return this._dataSetCollection.FirstOrDefault(ds => ds.DataSetId.ToLower() == dataSetId.ToLower());
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Could not locate the data set. {Error}", e);
            throw;
        }
    }

    public IDataSet? FindDataSetByName(string dataSetName)
    {
        try
        {
            if (string.IsNullOrEmpty(dataSetName))
                throw new ArgumentNullException(nameof(dataSetName));
        
            return this._dataSetCollection.FirstOrDefault(ds => ds.DataSetName.ToLower() == dataSetName.ToLower());
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Could not locate the data set. {Error}", e);
            throw;
        }
   
    }
    #endregion

    #region Private Methods

    private async Task RunCleanup()
    {
        await Abstractions.DependencyExtensions.Mediator?.Send(
            new ExpireViewsCommand())!;
    }

    private async void OnDataSetUpdated(object? sender, EventArgs e)
    {
        try
        {
            if (sender is not IDataSet dataSet)
                return;

            if (!_dataSetAttachedViews.ContainsKey(dataSet.DataSetId))
                return;

            if (!(_dataSetAttachedViews[dataSet.DataSetId]?.Count > 0)) return;
            
            _logger?.LogInformation("Data set with Id {Id} updated", dataSet.DataSetId);
            
            foreach (var viewId in _dataSetAttachedViews[dataSet.DataSetId])
            {
                _logger?.LogInformation("Updating view {ViewId} with data set Id {DataSetId}", viewId, dataSet.DataSetId);
                await DependencyExtensions.Mediator?.Send(
                    new Abstractions.CQRS.DataSetViews.Commands.UpdateViewFromDataSetCommand(viewId, dataSet))!;
            }
        }
        finally
        { 
            await this.RunCleanup(); 
        }
    }

    public void RemoveViewFromDataSets(string expiredViewId)
    {
        _logger?.LogWarning("Removing view with Id {Id} from all datasets", expiredViewId);
        lock (_dataSetAttachedViews)
        {
            _dataSetAttachedViews.Keys.ToList().ForEach(key =>
            {
                _dataSetAttachedViews[key].RemoveAll(x => string.Equals(x, key, StringComparison.OrdinalIgnoreCase));
            });
        }
    }
    #endregion
    
}