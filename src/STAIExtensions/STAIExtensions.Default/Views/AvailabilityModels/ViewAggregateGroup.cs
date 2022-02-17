using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Default.Helpers;

namespace STAIExtensions.Default.Views.AvailabilityModels;

public class ViewAggregateGroup
{

    #region Properties
    public string GroupName { get; private set; }
        
    public string FullGroupName { get; private set; }

    public List<ViewAggregate> Items { get; private set; } = new();

    public ViewAggregate? LastItem => Items?.OrderByDescending(i => i.EndDate).FirstOrDefault();
    public List<ViewAggregateGroup> Children { get; private set; } = new();
    #endregion

    #region ctor

    public ViewAggregateGroup(string groupName, string fullGroupName)
    {
        this.GroupName = groupName;
        this.FullGroupName = fullGroupName;
    }

    #endregion

    #region Methods

    internal void AddPeriodBoundaryData(PeriodBoundary<Availability> periodBoundary, IEnumerable<Availability> items)
    {
        var viewAggregate = new ViewAggregate(periodBoundary.StartDate, periodBoundary.EndDate);
        viewAggregate.CalculateGroupItems(items);
        this.Items.Add(viewAggregate);
    }
        
    internal void PushChildAggregate(ViewAggregateGroup runLocationAggregate)
    {
        Children.Add(runLocationAggregate);
    }
    #endregion
       
}