using STAIExtensions.Abstractions.CQRS.DataSets.Queries;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Core.Views;
using STAIExtensions.Default.DataSets;
using STAIExtensions.Default.Helpers;

namespace STAIExtensions.Default.Views;

public class AvailabilityView : DataSetView
{

    #region Constants

    private const string PARAM_CLOUDROLENAME = "CloudRoleName";
    private const string PARAM_CLOUDROLEINSTANCE = "CloudRoleInstance";
    private const string PARAM_STARTDATETIME = "StartDateTime";
    private const string PARAM_ENDDATETIME = "StartEndTime";
    private const string PARAM_GROUPINGMINUTES = "GroupingMinutes";

    #endregion

    #region Members

    private List<string>? _filterCloudRoleName;
    private List<string>? _filterCloudInstanceName;
    private DateTime? _startDateTime;
    private DateTime? _endDateTime;
    private int _periodGroupMinutes = 20;

    private List<Availability> _availabilityFiltered;
    private List<PeriodBoundary<Availability>> _periodBoundaries = new();

    #endregion

    #region Properties

    public int TotalItemsCount { get; private set; }

    public int FilteredItemsCount { get; private set; }
    
    public DateTime? MinTelemetryDate { get; private set; }
    
    public DateTime? MaxTelemetryDate { get; private set; }
    
    public Dictionary<string, List<string>> CloudNames { get; private set; } = new();

    public ViewAggregateGroup? AggregateGroup { get; private set; }
    #endregion

    #region Overrides

    public override IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors =>
        new List<DataSetViewParameterDescriptor>()
        {
            new DataSetViewParameterDescriptor(PARAM_CLOUDROLEINSTANCE, "string[]", false,
                "Filter values for the cloud role instance"),
            new DataSetViewParameterDescriptor(PARAM_CLOUDROLENAME, "string[]", false,
                "Filter values for the cloud role name"),
            new DataSetViewParameterDescriptor(PARAM_STARTDATETIME, "long", false, "Start UTC date time ticks"),
            new DataSetViewParameterDescriptor(PARAM_ENDDATETIME, "long", false, "End UTC date time ticks")
        };

    protected override Task BuildViewData(IDataSet dataSet)
    {
        if (dataSet is DataContractDataSet dataContractDataSet)
        {
            this.SetupParameters();

            this.LoadDistinctCloudNames(dataContractDataSet);

            this.BuildLocalListItems(dataContractDataSet);

            this.BuildViewItems(dataContractDataSet);
        }

        return Task.CompletedTask;
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Sets up the parameters for the view
    /// </summary>
    private void SetupParameters()
    {
        try
        {
            this._filterCloudRoleName =
                Helpers.ViewParameterHelper.ExtractParameter<List<string>>(this.ViewParameters, PARAM_CLOUDROLENAME);
            this._filterCloudInstanceName =
                Helpers.ViewParameterHelper.ExtractParameter<List<string>>(this.ViewParameters,
                    PARAM_CLOUDROLEINSTANCE);

            var startDateTimeTicks =
                Helpers.ViewParameterHelper.ExtractParameter<long?>(this.ViewParameters, PARAM_STARTDATETIME);
            var endDateTimeTicks =
                Helpers.ViewParameterHelper.ExtractParameter<long?>(this.ViewParameters, PARAM_ENDDATETIME);

            this._startDateTime = null;
            this._endDateTime = null;

            if (startDateTimeTicks is >= 0)
                this._startDateTime = new DateTime(startDateTimeTicks.Value);

            if (endDateTimeTicks is >= 0)
                this._endDateTime = new DateTime(endDateTimeTicks.Value);
            
            var groupingMinutes =  
                Helpers.ViewParameterHelper.ExtractParameter<int?>(this.ViewParameters, PARAM_GROUPINGMINUTES);

            if (groupingMinutes is >= 5)
                this._periodGroupMinutes = groupingMinutes.Value;

        }
        catch (Exception ex)
        {
            Abstractions.Common.ErrorLoggingFactory.LogError(base.TelemetryClient, base.Logger, ex,
                "An error occured deserializing the view parameters: {ErrorMessage}", ex.Message);
        }
    }

    /// <summary>
    /// Loads a list of distinct Cloud Role Instance and Cloud Role Name values
    /// </summary>
    /// <param name="dataContractDataSet">The data set containing the original items</param>
    private void LoadDistinctCloudNames(DataContractDataSet dataContractDataSet)
    {
        var result = new List<string?>();

        result.AddRange(dataContractDataSet.Availability.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}")
            .Distinct());

        var distinctNames = result.Select(x => x).Distinct().ToList();
        var distinctSplitItems = distinctNames.Select(n => n?.Split("*")).Where(x => x != null).Select(n => new
        {
            Instance = n[0],
            Role = n[1]
        }).ToList();

        this.CloudNames.Clear();
        distinctSplitItems.ForEach(item =>
        {
            if (!this.CloudNames.ContainsKey(item.Instance))
                this.CloudNames[item.Instance] = new List<string>();
            this.CloudNames[item.Instance].Add(item.Role);
        });
    }

    /// <summary>
    /// Filters the records by the view filters
    /// </summary>
    private void BuildLocalListItems(DataContractDataSet dataContractDataSet)
    {
        // Keep a copy of the lists before after it is filtered 
        this._availabilityFiltered = dataContractDataSet.Availability
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName)
            .FilterTimeStamp(this._startDateTime, this._endDateTime)
            .ToList();
    }

    /// <summary>
    /// Builds the output of the view items
    /// </summary>
    private void BuildViewItems(DataContractDataSet dataContractDataSet)
    {
        this.TotalItemsCount = dataContractDataSet.Availability.Count;
        this.FilteredItemsCount = this._availabilityFiltered.Count;

        this.SetupEntryPeriods();
        this.SetupGroupingPeriodBoundaries();
        
        this.AggregateGroup = BuildOverallGrouping();
    }

    private ViewAggregateGroup BuildOverallGrouping()
    {
        
        var viewAggregateGroup = new ViewAggregateGroup("Overall");
        var overallEntries = _availabilityFiltered?
            .ToArray();

        foreach (var period in _periodBoundaries)
        {
            var periodRecords = period.GetItems(overallEntries);
            viewAggregateGroup.AddPeriodBoundaryData(period, periodRecords);
        }

        var distinctTestNames = overallEntries?
            .Select(s => s.Name)
            .Distinct()
            .OrderBy(x => x);

        if (distinctTestNames == null) return viewAggregateGroup;
        foreach (var testName in distinctTestNames)
        {
            var runLocationAggregate = BuildTestNamesGroup(testName, overallEntries);
            viewAggregateGroup.PushChildAggregate(runLocationAggregate);
        }
        return viewAggregateGroup;
    }

    private ViewAggregateGroup BuildTestNamesGroup(string? testName, IEnumerable<Availability>? overallEntries)
    {
        var viewAggregateGroup = new ViewAggregateGroup(testName ?? "");
        var testNameEntries = overallEntries?
            .Where(e => string.Equals(e.Name, testName, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        foreach (var period in _periodBoundaries)
        {
            var periodRecords = period.GetItems(testNameEntries);
            viewAggregateGroup.AddPeriodBoundaryData(period, periodRecords);
        }

        var distinctRunLocations = testNameEntries?
            .Select(s => s.Location)
            .Distinct()
            .OrderBy(x => x);

        if (distinctRunLocations == null) return viewAggregateGroup;
        foreach (var runLocation in distinctRunLocations)
        {
            var runLocationAggregate = BuildRunLocationsGroup(runLocation, testNameEntries);
            viewAggregateGroup.PushChildAggregate(runLocationAggregate);
        }
        return viewAggregateGroup;
    }

    private ViewAggregateGroup BuildRunLocationsGroup(string? runLocation, IEnumerable<Availability>? testNameEntries)
    {
        var viewAggregateGroup = new ViewAggregateGroup(runLocation ?? "");
        var locationEntries = testNameEntries?
                .Where(e => string.Equals(e.Location, runLocation, StringComparison.OrdinalIgnoreCase))
                .ToArray();

        foreach (var period in _periodBoundaries)
        {
            var periodRecords = period.GetItems(locationEntries);
            viewAggregateGroup.AddPeriodBoundaryData(period, periodRecords);
        }

        return viewAggregateGroup;
    }

    private void SetupEntryPeriods()
    {
        this.MinTelemetryDate = null;
        this.MaxTelemetryDate = null;
        
        if (this._availabilityFiltered.Count > 0)
        {
            this.MinTelemetryDate = this._availabilityFiltered.Min(x => x.TimeStamp);
            this.MaxTelemetryDate = this._availabilityFiltered.Max(x => x.TimeStamp);
        }
    }

    private void SetupGroupingPeriodBoundaries()
    {
        if (this.MinTelemetryDate.HasValue == false || this.MaxTelemetryDate.HasValue == false) return;
        
        this._periodBoundaries.Clear();

        var workingDate = DateTime.UtcNow;
        
        while (workingDate > this.MinTelemetryDate.Value)
        {
            this._periodBoundaries.Add(new PeriodBoundary<Availability>(
                workingDate.AddMinutes(-this._periodGroupMinutes), 
                workingDate
            ));
            workingDate = workingDate.AddMinutes(-this._periodGroupMinutes);
        }
    }
    #endregion

    #region Model Classes

    public class ViewAggregateGroup
    {

        #region Properties
        public string GroupName { get; private set; }

        public List<ViewAggregate> Items { get; private set; } = new();

        public ViewAggregate? LastItem => Items?.Last();
        public List<ViewAggregateGroup> Children { get; private set; } = new();
        #endregion

        #region ctor

        public ViewAggregateGroup(string groupName)
        {
            this.GroupName = groupName;
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

    public class ViewAggregate
    {

        #region Properties
        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }
        
        public double MaxDuration { get; private set; }
        
        public double MinDuration { get; private set; }
        
        public double AverageDuration { get; private set; }
        
        public int SuccessfulCount { get; private set; } 

        public int FailureCount { get; private set; }
        
        public int TotalCount { get; private set; }
        
        public double SuccessPercentage { get; private set; }

        #endregion

        #region ctor

        internal ViewAggregate(DateTime startDate, DateTime endDate)
        {
            this.EndDate = endDate;
            this.StartDate = startDate;
        }

        #endregion

        #region Methods
        internal void CalculateGroupItems(IEnumerable<Availability>? source)
        {
            if (source == null || source?.Count() == 0) return;
            
            var availabilities = source as Availability[] ?? source.ToArray();
        
            this.MaxDuration =  availabilities.Select(s => s.Duration ?? 0).Max();
            this.MinDuration =  availabilities.Select(s => s.Duration ?? 0).Min();
            this.AverageDuration = availabilities.Select(s => s.Duration ?? 0).Average();
            this.SuccessfulCount = availabilities
                .Count(s => string.Equals(s.Success ?? "", "1", StringComparison.OrdinalIgnoreCase));
            this.FailureCount = availabilities
                .Count(s => !string.Equals(s.Success ?? "", "1", StringComparison.OrdinalIgnoreCase));
            this.TotalCount = availabilities.Length;
            this.SuccessPercentage = TotalCount == 0 ? 0 : ((double)SuccessfulCount * 100d) / (double)TotalCount;
        }
        #endregion

    }
    #endregion
  
}

