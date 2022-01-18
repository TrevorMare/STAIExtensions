using STAIExtensions.Abstractions.Data;
using STAIExtensions.Core.Views;
using STAIExtensions.Default.DataSets;

namespace STAIExtensions.Default.Views;

public class TelemetryOverview : DataSetView
{

    #region Properties

    public int? AvailabilityCount { get; private set; }
    
    public int? BrowserTimingsCount { get; private set; }
    
    public int? CustomEventsCount { get; private set; }
    
    public int? CustomMetricsCount { get; private set; }
    
    public int? DependenciesCount { get; private set; }
    
    public int? PageViewsCount { get; private set; }
    
    public int? PerformanceCountersCount { get; private set; }
    
    public int? RequestsCount { get; private set; }
    
    public int? TracesCount { get; private set; }
    #endregion

    #region Overrides
    
    protected override Task BuildViewData(IDataSet dataSet)
    {
        if (dataSet is DataContractDataSet dataContractDataSet)
        {
            this.LoadTotals(dataContractDataSet);
        }
        return Task.CompletedTask;
    }
    #endregion

    #region Private Medhods

    private void LoadTotals(DataContractDataSet dataContractDataSet)
    {
        this.AvailabilityCount = dataContractDataSet.Availability.Count;
        this.BrowserTimingsCount = dataContractDataSet.BrowserTiming.Count;
        this.CustomEventsCount = dataContractDataSet.CustomEvents.Count;
        this.CustomMetricsCount = dataContractDataSet.CustomMetrics.Count;
        this.DependenciesCount = dataContractDataSet.Dependencies.Count;
        this.PageViewsCount = dataContractDataSet.PageViews.Count;
        this.PerformanceCountersCount = dataContractDataSet.PerformanceCounters.Count;
        this.RequestsCount = dataContractDataSet.Requests.Count;
        this.TracesCount = dataContractDataSet.Traces.Count;
    }
    

    private void LoadDistinctRoleNames(DataContractDataSet dataContractDataSet)
    {
        var result = new List<string>();
        
        from q in dataContractDataSet.Availability

    }

    #endregion
    
    
}