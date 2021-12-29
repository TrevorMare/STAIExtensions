using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Collections;

public interface IDataSetCollection
{

    delegate void OnDataSetUpdatedHandler(IDataSet sender, EventArgs e);

    event OnDataSetUpdatedHandler OnDataSetUpdated;
    
    bool AttachDataSet(IDataSet dataSet);
    
    bool DetachDataSet(IDataSet dataSet);

    IEnumerable<DataSetInformation> ListDataSets();

    bool AttachViewToDataSet(string dataSetId, IDataSetView view);
    
    bool DetachViewFromDataSet(string dataSetId, IDataSetView view);

    IDataSet? FindDataSetById(string dataSetId);
    
    IDataSet? FindDataSetByName(string dataSetId);
    
    void RemoveViewFromDataSets(string expiredViewId);
 
    IEnumerable<IDataSet>? FindDataSetsByViewId(string requestViewId);
}