using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Core.Views;

public class MyErrorsView : DataSetView
{

    public int? TotalErrorCount { get; private set; } = null;

    public IEnumerable<Abstractions.DataContracts.Models.AIException>? LastErrors { get; private set; }

    public override Task OnDataSetUpdated(IDataSet dataset)
    {
        if (dataset is DataSets.DataContractDataSet dataContractDataSet)
        {
            this.TotalErrorCount = dataContractDataSet.Exceptions.Count();
            this.LastErrors = dataContractDataSet.Exceptions.OrderByDescending(x => x.TimeStamp).Take(5);
        }
        
        return base.OnDataSetUpdated(dataset);
    }
    
    
    
}