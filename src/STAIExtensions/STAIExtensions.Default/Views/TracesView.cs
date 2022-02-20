using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Core.Views;
using STAIExtensions.Default.DataSets;
using STAIExtensions.Default.Helpers;

namespace STAIExtensions.Default.Views;

public class TracesView: BaseView
{
    
    #region Members
    private List<Trace> _tracesFiltered = new();
    #endregion

    #region Properties
    public List<Trace> TraceItems { get; private set; } = new();
    #endregion

    #region ctor

    public TracesView()
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
        this._tracesFiltered = dataContractDataSet.Traces
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames)
            .ToList();
    }

    /// <summary>
    /// Builds the output of the view items
    /// </summary>
    private void BuildViewItems(DataContractDataSet dataContractDataSet)
    {
        this.TraceItems = this._tracesFiltered
            .Where(t => t.RecordState == RecordState.New)
            .OrderByDescending(x => x.TimeStamp)
            .Take(200)
            .OrderBy(x => x.TimeStamp)
            .ToList();
    }
    #endregion
    
   
}