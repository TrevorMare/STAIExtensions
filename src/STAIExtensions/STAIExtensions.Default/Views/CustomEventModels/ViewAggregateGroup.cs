using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Default.Helpers;

namespace STAIExtensions.Default.Views.CustomEventModels;

public class ViewAggregateGroup
{
    public string? GroupName { get; set; } = null;

    public List<ViewAggregate> Items { get; private set; } = new();

    internal void AddPeriodBoundaryData(PeriodBoundary<CustomEvent> period, IEnumerable<CustomEvent> periodGroupRecords)
    {
        this.Items.Add(new ViewAggregate(periodGroupRecords.Count(), period.StartDate, period.EndDate));
    }
}