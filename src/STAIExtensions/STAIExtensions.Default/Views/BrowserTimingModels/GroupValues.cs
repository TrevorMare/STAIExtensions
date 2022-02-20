using System.Linq.Expressions;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Default.Views.BrowserTimingModels;

public class GroupValues
{

    #region Properties

    public List<GroupedStatistics> Statistics { get; set; } = new();

    #endregion

    #region Methods

    public void CalculateGroupedStatistics(List<BrowserTiming> items, 
                                           Func<BrowserTiming, string> groupSelector)
    {

        var itemGroups = items.GroupBy(groupSelector);
        
        foreach (var itemGroup in itemGroups)
        {
            List<BrowserTiming> groupItems = itemGroup.ToList();
            
            var groupedStatistics = new GroupedStatistics(itemGroup.Key);
            groupedStatistics.BuildStatistics(groupItems);
            
            this.Statistics.Add(groupedStatistics);
        }
    }
    #endregion
    
}