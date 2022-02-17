using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Default.DataSets;
using STAIExtensions.Default.Helpers;
using STAIExtensions.Default.Views.BrowserTimingModels;

namespace STAIExtensions.Default.Views;

public class BrowserTimingsView : BaseView
{

    #region Members

    private List<BrowserTiming> _browserTimingsFiltered = new();

    #endregion
    
    #region Properties
    public int? TotalNumberOfItems { get; private set; }

    public int? FilteredNumberOfItems { get; private set; }

    public IEnumerable<BrowserTiming> LastItems { get; private set; }

    public GroupValues ClientBrowserStatistics { get; private set; }

    public GroupValues ClientCityStatistics { get; private set; }
    
    public GroupValues CountryOrRegionStatistics { get; private set; }
    
    public GroupValues UserSessionStatistics { get; private set; }
    
    public GroupValues UserIdStatistics { get; private set; }
    
    public GroupValues OperationNameStatistics { get; private set; }
    #endregion

    #region ctor

    public BrowserTimingsView()
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
        };

    #endregion
    
    #region Build Methods
    protected virtual Task BuildViewDataFromDataSet(DataContractDataSet dataSet)
    {
        try
        {
            this.SetupParameters();

            this.SetupDistinctCloudInstanceAndRoleNames(new List<IEnumerable<DataContractFull>>()
            {
                dataSet.BrowserTiming
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
        this._browserTimingsFiltered = dataContractDataSet.BrowserTiming
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames)
            .ToList();
    }

    /// <summary>
    /// Builds the output of the view items
    /// </summary>
    private void BuildViewItems(DataContractDataSet dataContractDataSet)
    {
        // Set the total number of items
        this.TotalNumberOfItems = dataContractDataSet.BrowserTiming.Count;
        // Set the number of filtered items
        this.FilteredNumberOfItems = _browserTimingsFiltered.Count;
        // Get the last items from the result set
        this.LastItems = this._browserTimingsFiltered
            .OrderByDescending(x => x.TimeStamp)
            .Take(10)
            .OrderBy(x => x.TimeStamp)
            .ToList();
        
        // Calculate the statistics by groups
        CalculateStatisticsByClientBrowser();
        CalculateStatisticsByClientCity();
        CalculateStatisticsByCountryOrRegion();
        CalculateStatisticsByUserSession();
        CalculateStatisticsByUserId();
        CalculateStatisticsByOperationName();
    }

    /// <summary>
    /// Calculate the statistics by Client Browser
    /// </summary>
    private void CalculateStatisticsByClientBrowser()
    {
        this.ClientBrowserStatistics = new GroupValues();
        this.ClientBrowserStatistics.CalculateGroupedStatistics(this._browserTimingsFiltered,
            timing => timing.ClientBrowser);
    }
    
    /// <summary>
    /// Calculate the statistics by Client City
    /// </summary>
    private void CalculateStatisticsByClientCity()
    {
        this.ClientCityStatistics = new GroupValues();
        this.ClientCityStatistics.CalculateGroupedStatistics(this._browserTimingsFiltered,
            timing => timing.ClientCity);
    }
    
    /// <summary>
    /// Calculate the statistics by Country or Region
    /// </summary>
    private void CalculateStatisticsByCountryOrRegion()
    {
        this.CountryOrRegionStatistics = new GroupValues();
        this.CountryOrRegionStatistics.CalculateGroupedStatistics(this._browserTimingsFiltered,
            timing => timing.ClientCountryOrRegion);
    }
    
    /// <summary>
    /// Calculate the statistics by User Session
    /// </summary>
    private void CalculateStatisticsByUserSession()
    {
        this.UserSessionStatistics = new GroupValues();
        this.UserSessionStatistics.CalculateGroupedStatistics(this._browserTimingsFiltered,
            timing => timing.SessionId);
    }
    
    /// <summary>
    /// Calculate the statistics by User Session
    /// </summary>
    private void CalculateStatisticsByUserId()
    {
        this.UserIdStatistics = new GroupValues();
        this.UserIdStatistics.CalculateGroupedStatistics(this._browserTimingsFiltered,
            timing => timing.UserId);
    }
    
    /// <summary>
    /// Calculate the statistics by User Session
    /// </summary>
    private void CalculateStatisticsByOperationName()
    {
        this.OperationNameStatistics = new GroupValues();
        this.OperationNameStatistics.CalculateGroupedStatistics(this._browserTimingsFiltered,
            timing => timing.OperationName);
    }
    #endregion
 
}