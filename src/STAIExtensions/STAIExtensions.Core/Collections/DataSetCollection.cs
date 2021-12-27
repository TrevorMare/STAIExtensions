using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

public class DataSetCollection : Abstractions.Collections.IDataSetCollection
{

    #region Members
    private readonly List<IDataSet> _dataSetCollection = new();
    private readonly Dictionary<string, List<string>> _dataSetAttachedViews = new();
    private readonly ILogger<DataSetCollection>? _logger;
    #endregion

    #region ctor
    public DataSetCollection()
    {
        _logger = DependencyExtensions.CreateLogger<DataSetCollection>();
        
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


            dataSet.OnDataSetUpdated += OnDataSetUpdated;
        
            _dataSetAttachedViews[dataSet.DataSetId] = new List<string>();
            _dataSetCollection.Add(dataSet);
            
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
        
            _dataSetAttachedViews[dataSet.DataSetId] = new List<string>();
            _dataSetCollection.Remove(dataSet);
        
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

            if (!_dataSetAttachedViews[dataSetId].Contains(view.Id.ToLower()))
            {
                _dataSetAttachedViews[dataSetId].Add(view.Id.ToLower());
                _logger?.LogInformation(
                    "View with Id {ViewId} is now attached to data set {DataSetId}",
                    view.Id, dataSet.DataSetId);
                return true;
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
        
            if (_dataSetAttachedViews[dataSetId].Contains(view.Id.ToLower()))
            {
                _dataSetAttachedViews[dataSetId].Remove(view.Id.ToLower());
                _logger?.LogInformation(
                    "View with Id {ViewId} is now detached from data set {DataSetId}",
                    view.Id, dataSet.DataSetId);
                return true;
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

    private async void OnDataSetUpdated(object? sender, EventArgs e)
    {
        if (sender is not IDataSet dataSet)
            return;
        
        _logger?.LogInformation("Data set with Id {Id} updated", dataSet.DataSetId);
        
        foreach (var viewId in _dataSetAttachedViews[dataSet.DataSetId])
        {
            _logger?.LogInformation("Updating view {ViewId} with data set Id {DataSetId}", viewId, dataSet.DataSetId);
            await DependencyExtensions.Mediator?.Send(
                new Abstractions.CQRS.DataSetViews.Commands.UpdateViewFromDataSetCommand(viewId, dataSet))!;
        }        
    }
    #endregion
    
}