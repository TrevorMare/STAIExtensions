using STAIExtensions.Abstractions.CQRS.DataSets.Queries;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Core.Views;
using STAIExtensions.Default.DataSets;
using STAIExtensions.Default.Helpers;
using STAIExtensions.Default.Views.AvailabilityModels;

namespace STAIExtensions.Default.Views;

public class AvailabilityView : BaseView
{

    #region Constants
    private const string PARAM_STARTDATETIME = "StartDateTime";
    private const string PARAM_ENDDATETIME = "StartEndTime";
    private const string PARAM_GROUPINGMINUTES = "GroupingMinutes";
    #endregion

    #region Members

    private DateTime? _startDateTime;
    private DateTime? _endDateTime;
    private int _periodGroupMinutes = 20;

    private List<Availability> _availabilityFiltered = new();
    private List<PeriodBoundary<Availability>> _periodBoundaries = new();

    #endregion

    #region Properties

    /// <summary>
    /// Gets the total number of items before filtering applied
    /// </summary>
    public int TotalItemsCount { get; private set; }

    /// <summary>
    /// Gets the number of items after the filtering applied
    /// </summary>
    public int FilteredItemsCount { get; private set; }
    
    /// <summary>
    /// Gets the minimum date of the filtered telemetry items
    /// </summary>
    public DateTime? MinTelemetryDate { get; private set; }
    
    /// <summary>
    /// Gets the maximum date of the filtered telemetry items
    /// </summary>
    public DateTime? MaxTelemetryDate { get; private set; }

    /// <summary>
    /// Gets the view aggregate group details of the filtered telemetry items
    /// </summary>
    public ViewAggregateGroup? AggregateGroup { get; private set; }
    #endregion

    #region ctor

    public AvailabilityView()
    {
        RegisterDataSetTypeForUpdates<DataContractDataSet>(set =>
        {
            BuildViewDataFromDataSet(set);
        });
    }

    #endregion
   
    
    #region Overrides

    /// <summary>
    /// Returns the View acceptable parameters
    /// </summary>
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
     #endregion

     #region Build Methods

     /// <summary>
     /// Builds the view details
     /// </summary>
     /// <param name="dataSet"></param>
     /// <returns></returns>
     protected virtual Task BuildViewDataFromDataSet(DataContractDataSet dataSet)
     {

         try
         {
             this.SetupParameters();

             this.SetupDistinctCloudInstanceAndRoleNames(new List<IEnumerable<DataContractFull>>()
             {
                 dataSet.Availability
             });

             this.BuildLocalListItems(dataSet);

             this.BuildViewItems(dataSet);
         }
         catch (Exception e)
         {
             Abstractions.Common.ErrorLoggingFactory.LogError(this.TelemetryClient, this.Logger, e,
                 "An error occured updating the view: {ErrorMessage}", e.Message);
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
           this.SetupCloudRoleAndInstanceNamesFromParameters();

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
    /// Filters the records by the view filters
    /// </summary>
    private void BuildLocalListItems(DataContractDataSet dataContractDataSet)
    {
        // Keep a copy of the lists before after it is filtered 
        this._availabilityFiltered = dataContractDataSet.Availability
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames)
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
        
        var viewAggregateGroup = new ViewAggregateGroup("Overall", "Overall");
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
        var viewAggregateGroup = new ViewAggregateGroup(testName ?? "", $"Overall-{testName}");
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
            var runLocationAggregate = BuildRunLocationsGroup(testName, runLocation, testNameEntries);
            viewAggregateGroup.PushChildAggregate(runLocationAggregate);
        }
        return viewAggregateGroup;
    }

    private ViewAggregateGroup BuildRunLocationsGroup(string? testName, string? runLocation, IEnumerable<Availability>? testNameEntries)
    {
        var viewAggregateGroup = new ViewAggregateGroup(runLocation ?? "", $"Overall-{testName}-{runLocation}");
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

 
  
}

