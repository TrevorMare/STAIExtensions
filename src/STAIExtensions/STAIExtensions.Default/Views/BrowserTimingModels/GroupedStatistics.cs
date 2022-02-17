using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Default.Views.BrowserTimingModels;

public class GroupedStatistics
{

    #region Properties
    public StatisticValues? TotalValues { get; private set; }

    public StatisticValues? NetworkValues { get; private set; }
    
    public StatisticValues? ProcessingValues { get; private set; }
    
    public StatisticValues? ReceiveValues { get; private set; }
    
    public StatisticValues? SendValues { get; private set; }

    public int? NumberOfItems { get; private set; }

    public string? GroupName { get; private set; }

    public IEnumerable<string?>? SlowestOperations { get; set; }

    public IEnumerable<string?>? FastestOperations { get; set; }
    #endregion

    #region ctor
    public GroupedStatistics(string? groupName)
    {
        this.GroupName = groupName;
    }
    #endregion

    #region Public Methods
    public void BuildStatistics(List<BrowserTiming>? groupItems)
    {
        if (groupItems == null || groupItems.Any() == false) return;
        
        this.NumberOfItems = groupItems.Count;

        // Calculate the total durations
        this.TotalValues = new StatisticValues(groupItems.Select(x => x.TotalDuration).ToList());
        this.NetworkValues = new StatisticValues(groupItems.Select(x => x.NetworkDuration).ToList());
        this.ProcessingValues = new StatisticValues(groupItems.Select(x => x.ProcessingDuration).ToList());
        this.ReceiveValues = new StatisticValues(groupItems.Select(x => x.ReceiveDuration).ToList());
        this.SendValues = new StatisticValues(groupItems.Select(x => x.SendDuration).ToList());

        // Group the items by URL
        this.CalculateStatisticsByOperationName(groupItems);
    }
    #endregion

    #region Private Methods

    private void CalculateStatisticsByOperationName(List<BrowserTiming> groupItems)
    {
        var distinctUrls = groupItems.Select(x => x.Url).Distinct();
        
        // Calculate the statistics for each group
        var urlAverageItems = groupItems
            .GroupBy(x => x.OperationName)
            .Select(x => new
            {
                GroupName = x.Key, 
                AverageTotalDuration = x.Average(y => y.TotalDuration),
                AverageNetworkDuration = x.Average(y => y.NetworkDuration)
            }).ToList();

        SlowestOperations = urlAverageItems
            .OrderBy(x => x.AverageTotalDuration)
            .Take(3)
            .Select(x => x.GroupName);
        
        FastestOperations = urlAverageItems
            .OrderByDescending(x => x.AverageTotalDuration)
            .Take(3)
            .Select(x => x.GroupName);
    }
    #endregion
        
}