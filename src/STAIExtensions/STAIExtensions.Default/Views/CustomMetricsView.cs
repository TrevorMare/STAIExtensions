using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Default.DataSets;
using STAIExtensions.Default.Helpers;
using STAIExtensions.Default.Views.CustomMetricModels;

namespace STAIExtensions.Default.Views;

public class CustomMetricsView : BaseView
{
    
    #region Constants

    private const string PARAM_LASTRECORDCOUNT = "LastRecordCount"; 
    private const string PARAM_STARTDATETIME = "StartDateTime";
    private const string PARAM_ENDDATETIME = "StartEndTime";
    private const string PARAM_GROUPINGMINUTES = "GroupingMinutes";

    #endregion
    
    #region Members
    private List<CustomMetric> _customMetricsFiltered = new();
    private List<PeriodBoundary<CustomMetric>> _periodBoundaries = new();
    
    private int _lastRecordCount = 10;
    private DateTime? _startDateTime;
    private DateTime? _endDateTime;
    private int _periodGroupMinutes = 600;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the minimum date of the filtered telemetry items
    /// </summary>
    public DateTime? MinTelemetryDate { get; private set; }
    
    /// <summary>
    /// Gets the maximum date of the filtered telemetry items
    /// </summary>
    public DateTime? MaxTelemetryDate { get; private set; }
    
    /// <summary>
    /// Gets the number of total items
    /// </summary>
    public int? TotalNumberOfItems { get; private set; }
    
    /// <summary>
    /// Gets the number of filtered items
    /// </summary>
    public int? FilteredNumberOfItems { get; private set; }
    
    /// <summary>
    /// Gets the latest number of entries
    /// </summary>
    public List<CustomMetric> LastCustomMetricItems { get; private set; } = new();

    public List<ViewAggregateGroup> AggregateGroups { get; private set; } = new();

    #endregion

    #region ctor

    public CustomMetricsView()
    {
        RegisterDataSetTypeForUpdates<DataContractDataSet>(set =>
        {
            BuildViewDataFromDataSet(set);
        });
    }

    #endregion
    
    #region Overrides

    public override IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors =>
        new List<DataSetViewParameterDescriptor>()
        {
            new DataSetViewParameterDescriptor(PARAM_CLOUDROLEINSTANCE, "string[]", false,
                "Filter values for the cloud role instance"),
            new DataSetViewParameterDescriptor(PARAM_CLOUDROLENAME, "string[]", false,
                "Filter values for the cloud role name"),
            new DataSetViewParameterDescriptor(PARAM_LASTRECORDCOUNT, "int", false, 
                "Number of records to load for the last entities"),
            new DataSetViewParameterDescriptor(PARAM_STARTDATETIME, "long", false, 
                "Start UTC date time ticks"),
            new DataSetViewParameterDescriptor(PARAM_ENDDATETIME, "long", false, 
                "End UTC date time ticks"),
            new DataSetViewParameterDescriptor(PARAM_GROUPINGMINUTES, "int", false,
                "Number of minutes to group by")
        };

    #endregion

    #region Build Methods

    protected virtual Task BuildViewDataFromDataSet(DataContractDataSet dataSet)
    {
        try
        {
            this.SetupDistinctCloudInstanceAndRoleNames(new List<IEnumerable<DataContractFull>>()
            {
                dataSet.Traces
            });

            this.BuildLocalListItems(dataSet);

            this.BuildViewItems(dataSet);

        }
        catch (Exception e)
        {
            Abstractions.Common.ErrorLoggingFactory.LogError(this.TelemetryClient, this.Logger, e,
                "An error occured updating the view: {ErrorMessage}", e.Message);
        }        this.SetupParameters();

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
            
            var lastRecordCount =
                Helpers.ViewParameterHelper.ExtractParameter<int?>(this.ViewParameters, PARAM_LASTRECORDCOUNT);
            
            if (lastRecordCount.HasValue && lastRecordCount >= 0)
            {
                this._lastRecordCount = lastRecordCount.Value;
            }
            
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
        this._customMetricsFiltered = dataContractDataSet.CustomMetrics
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
        this.LastCustomMetricItems = this._customMetricsFiltered
            .OrderByDescending(x => x.TimeStamp)
            .Take(_lastRecordCount)
            .OrderBy(x => x.TimeStamp)
            .ToList();
        this.TotalNumberOfItems = dataContractDataSet.CustomMetrics.Count;
        this.FilteredNumberOfItems = this._customMetricsFiltered.Count;

        this.SetupEntryPeriods();
        this.SetupGroupingPeriodBoundaries();

        this.BuildGroupValues();
    }

    private void BuildGroupValues()
    {
        var operationNameGroupValues = _customMetricsFiltered
            .Select(x => x.Name)
            .OrderBy(x => x)
            .Distinct();

        AggregateGroups.Clear();
        
        foreach (var operationNameGroupValue in operationNameGroupValues)
        {
            
            var operationGroupItems = _customMetricsFiltered
                .Where(x => x.Name == operationNameGroupValue)
                .ToList();

            var operationAggregateGroup = new ViewAggregateGroup()
            {
                GroupName = operationNameGroupValue
            };
            
            foreach (var period in _periodBoundaries)
            {
                var periodRecords = period.GetItems(operationGroupItems);
                operationAggregateGroup.AddPeriodBoundaryData(period, periodRecords);
            }

            AggregateGroups.Add(operationAggregateGroup);
        }
    }
    
    private void SetupEntryPeriods()
    {
        this.MinTelemetryDate = null;
        this.MaxTelemetryDate = null;
        
        if (this._customMetricsFiltered.Count > 0)
        {
            this.MinTelemetryDate = this._customMetricsFiltered.Min(x => x.TimeStamp);
            this.MaxTelemetryDate = this._customMetricsFiltered.Max(x => x.TimeStamp);
        }
    }
    
    private void SetupGroupingPeriodBoundaries()
    {
        if (this.MinTelemetryDate.HasValue == false || this.MaxTelemetryDate.HasValue == false) return;
        
        this._periodBoundaries.Clear();

        var workingDate = DateTime.UtcNow;
        
        while (workingDate > this.MinTelemetryDate.Value)
        {
            this._periodBoundaries.Add(new PeriodBoundary<CustomMetric>(
                workingDate.AddMinutes(-this._periodGroupMinutes), 
                workingDate
            ));
            workingDate = workingDate.AddMinutes(-this._periodGroupMinutes);
        }
    }
    #endregion
}