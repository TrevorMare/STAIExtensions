using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

public class DataSetCollection : Abstractions.Collections.IDataSetCollection
{

    #region Members
    private List<IDataSet> _dataSetCollection = new();
    #endregion

    #region Methods
    public bool AttachDataSet(IDataSet dataSet)
    {
        if (dataSet == null)
            throw new ArgumentNullException(nameof(dataSet));

        if (_dataSetCollection.Contains(dataSet))
            return false;
        
        _dataSetCollection.Add(dataSet);
        
        return true;
    }

    public bool DetachDataSet(IDataSet dataSet)
    {
        if (dataSet == null)
            throw new ArgumentNullException(nameof(dataSet));

        if (!_dataSetCollection.Contains(dataSet))
            return false;
        
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
        
        dataSet.AttachView(view);
        return true;
    }

    public bool DetachViewFromDataSet(string dataSetId, IDataSetView view)
    {
        if (view == null)
            throw new ArgumentNullException(nameof(view));
        
        var dataSet = this.FindDataSetById(dataSetId);
        if (dataSet == null)
            return false;
        
        dataSet.DetachView(view);
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
    
}