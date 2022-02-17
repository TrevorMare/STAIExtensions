using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Core.Views;
using STAIExtensions.Default.DataSets;
using STAIExtensions.Default.Helpers;
using STAIExtensions.Default.Interfaces;

namespace STAIExtensions.Default.Views;

public abstract class BaseView : DataSetView
{

    #region Constants
    public const string PARAM_CLOUDROLENAME = "CloudRoleName";
    public const string PARAM_CLOUDROLEINSTANCE = "CloudRoleInstance";
    #endregion

    #region Members

    protected List<string>? FilterCloudRoleNames;
    protected List<string>? FilterCloudInstanceNames;

    private List<IRegisteredDataSet> _registeredDataSets = new();
    #endregion

    #region Properties
    public Dictionary<string, List<string>> CloudNames { get; protected set; } = new();
    

    #endregion

    #region Methods

    protected void RegisterDataSetTypeForUpdates<T>(Action<T> updateAction) where T : IDataSet
    {
        var dataSetRegistration = new RegisteredDataSet<T>(updateAction);
        
        this._registeredDataSets.Add(dataSetRegistration);
    }

    protected override Task BuildViewData(IDataSet dataSet)
    {
        foreach (var registeredDataSet in _registeredDataSets)
        {
            var registeredType = registeredDataSet.DataSetType;
            if (dataSet.GetType() == registeredType)
            {
                registeredDataSet.UpdateAction?.DynamicInvoke(dataSet);
            }
        }

        return Task.CompletedTask;
    }
    

    protected virtual void SetupCloudRoleAndInstanceNamesFromParameters()
    {
        try
        {
            this.FilterCloudRoleNames =
                Helpers.ViewParameterHelper.ExtractParameter<List<string>>(this.ViewParameters, PARAM_CLOUDROLENAME);
            this.FilterCloudInstanceNames =
                Helpers.ViewParameterHelper.ExtractParameter<List<string>>(this.ViewParameters,
                    PARAM_CLOUDROLEINSTANCE);
        }
        catch (Exception e)
        {
            base.TelemetryClient?.TrackException(e);
        }
    }

    protected virtual void SetupDistinctCloudInstanceAndRoleNames(IEnumerable<IEnumerable<DataContractFull>> items)
    {
        var result = new List<string?>();
        
        foreach (var dataContractFull in items)
        {
            result.AddRange(dataContractFull.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}")
                .Distinct());
        }

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

    #endregion
    
}