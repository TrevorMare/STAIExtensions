using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Core.Views;
using STAIExtensions.Default.DataSets;

namespace STAIExtensions.Default.Views;

public class MyErrorsView : DataSetView
{
    public override IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors =>
        new List<DataSetViewParameterDescriptor>()
        {
            new DataSetViewParameterDescriptor("CloudRole", "string", false),
            new DataSetViewParameterDescriptor("CloudInstance", "string", false)
        };

    public int? TotalErrorCount { get; private set; } = null;

    public IEnumerable<Abstractions.DataContracts.Models.AIException>? LastErrors { get; private set; }

    protected override Task BuildViewData(IDataSet dataSet)
    {
        if (dataSet is DataContractDataSet dataContractDataSet)
        {
            this.TotalErrorCount = dataContractDataSet.Exceptions.Count();
            this.LastErrors = dataContractDataSet.Exceptions.OrderByDescending(x => x.TimeStamp).Take(5);
        }
        return Task.CompletedTask;
    }
}