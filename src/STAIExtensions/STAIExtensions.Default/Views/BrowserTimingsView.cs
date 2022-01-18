using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Core.Views;
using STAIExtensions.Default.DataSets;

namespace STAIExtensions.Default.Views;

public class BrowserTimingsView : DataSetView
{
    public int? NumberOfItems { get; set; }

    public IEnumerable<BrowserTiming> TopItems { get; set; }
    
    protected override Task BuildViewData(IDataSet dataSet)
    {
        if (dataSet is DataContractDataSet dataContractDataSet)
        {
            this.NumberOfItems = dataContractDataSet.BrowserTiming.Count;
            this.TopItems = dataContractDataSet.BrowserTiming.OrderByDescending(x => x.TimeStamp).Take(5);
        }
        return Task.CompletedTask;
    }
}