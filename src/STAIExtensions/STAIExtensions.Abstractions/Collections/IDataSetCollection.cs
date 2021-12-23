using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Collections;

public interface IDataSetCollection
{

    void AttachDataSet(IDataSet dataSet);
    
    void DetachDataSet(IDataSet dataSet);

    IEnumerable<string> ListDataSets();

    bool AttachViewToDataSet(string requestDataSetId, IDataSetView view);
    
    bool DetachViewFromDataSet(string requestDataSetId, IDataSetView view);
}