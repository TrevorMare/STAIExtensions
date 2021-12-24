using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

public class DataSetCollection : Abstractions.Collections.IDataSetCollection
{

    #region Members
    private readonly List<IDataSet> _dataSetCollection = new();
    private readonly Dictionary<string, List<string>> _dataSetAttachedViews = new();
    #endregion

    #region Methods
    public bool AttachDataSet(IDataSet dataSet)
    {
        if (dataSet == null)
            throw new ArgumentNullException(nameof(dataSet));

        if (_dataSetCollection.Contains(dataSet))
            return false;


        dataSet.OnDataSetUpdated += OnDataSetUpdated;
        
        _dataSetAttachedViews[dataSet.DataSetId] = new List<string>();
        _dataSetCollection.Add(dataSet);
        
        return true;
    }

    public bool DetachDataSet(IDataSet dataSet)
    {
        if (dataSet == null)
            throw new ArgumentNullException(nameof(dataSet));

        if (!_dataSetCollection.Contains(dataSet))
            return false;
        
        dataSet.OnDataSetUpdated -= OnDataSetUpdated;
        
        _dataSetAttachedViews[dataSet.DataSetId] = new List<string>();
        _dataSetCollection.Remove(dataSet);
        
        return true;
    }

    public IEnumerable<DataSetInformation> ListDataSets()
    {
        return _dataSetCollection.Select(dataSet => new DataSetInformation(dataSet.DataSetName, dataSet.DataSetId, dataSet.DataSetType));
    }

    public bool AttachViewToDataSet(string dataSetId, IDataSetView view)
    {
        if (view == null)
            throw new ArgumentNullException(nameof(view));
        
        var dataSet = this.FindDataSetById(dataSetId);
        if (dataSet == null)
            return false;

        if (!_dataSetAttachedViews[dataSetId].Contains(view.Id.ToLower()))
        {
            _dataSetAttachedViews[dataSetId].Add(view.Id.ToLower());
        }

        return true;
    }

    public bool DetachViewFromDataSet(string dataSetId, IDataSetView view)
    {
        if (view == null)
            throw new ArgumentNullException(nameof(view));
        
        var dataSet = this.FindDataSetById(dataSetId);
        if (dataSet == null)
            return false;
        
        if (_dataSetAttachedViews[dataSetId].Contains(view.Id.ToLower()))
        {
            _dataSetAttachedViews[dataSetId].Remove(view.Id.ToLower());
        }

        return true;
    }

    public IDataSet? FindDataSetById(string dataSetId)
    {
        if (string.IsNullOrEmpty(dataSetId))
            throw new ArgumentNullException(nameof(dataSetId));
        
        return this._dataSetCollection.FirstOrDefault(ds => ds.DataSetId?.ToLower() == dataSetId?.ToLower());
    }

    public IDataSet? FindDataSetByName(string dataSetName)
    {
        if (string.IsNullOrEmpty(dataSetName))
            throw new ArgumentNullException(nameof(dataSetName));
        
        return this._dataSetCollection.FirstOrDefault(ds => ds.DataSetName?.ToLower() == dataSetName?.ToLower());
    }
    #endregion

    #region Private Methods

    private async void OnDataSetUpdated(object? sender, EventArgs e)
    {
        if (sender is not IDataSet dataSet)
            return;
        
        foreach (var viewId in _dataSetAttachedViews[dataSet.DataSetId])
        {
            await Abstractions.DependencyExtensions.Mediator?.Send(
                new Abstractions.CQRS.DataSetViews.Commands.UpdateViewFromDataSetCommand(viewId, dataSet))!;
        }        
    }
    #endregion
    
}