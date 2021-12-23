using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

public class DataSetCollection : Abstractions.Collections.IDataSetCollection
{
    public void AttachDataSet(IDataSet dataSet)
    {
        throw new NotImplementedException();
    }

    public void DetachDataSet(IDataSet dataSet)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> ListDataSets()
    {
        throw new NotImplementedException();
    }

    public bool AttachViewToDataSet(string requestDataSetId, IDataSetView view)
    {
        throw new NotImplementedException();
    }

    public bool DetachViewFromDataSet(string requestDataSetId, IDataSetView view)
    {
        throw new NotImplementedException();
    }
}