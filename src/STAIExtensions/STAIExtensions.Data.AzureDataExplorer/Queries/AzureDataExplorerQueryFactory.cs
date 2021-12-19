using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.DataContracts;


namespace STAIExtensions.Data.AzureDataExplorer.Queries;

public static class AzureDataExplorerQueryFactory
{
    #region Methods

    public static string GetDataContractSourceTableName(Abstractions.Common.DataContractSource source)
    {
        return source.DisplayName();
    }
    
    // public static AzureDataExplorerQuery<IDataContract> BuildQuery(
    //     Abstractions.Common.DataContractSource source, int? topRows,
    //     bool? orderByTimestampDesc) 
    // {
    //     var tableName = source.DisplayName();
    //     AzureDataExplorerQuery<IDataContract> result = null; 
    //     switch (source)
    //     {
    //         case DataContractSource.Availability:
    //             var x = BuildCustomQuery<IAvailability>(tableName, tableName, topRows, orderByTimestampDesc) as AzureDataExplorerQuery<IDataContract>;
    //             
    //             break;
    //         case DataContractSource.BrowserTiming:
    //             break;
    //         case DataContractSource.CustomEvent:
    //             break;
    //         case DataContractSource.CustomMetric:
    //             break;
    //         case DataContractSource.Dependency:
    //             break;
    //         case DataContractSource.Exception:
    //             break;
    //         case DataContractSource.PageViews:
    //             break;
    //         case DataContractSource.PerformanceCounter:
    //             break;
    //         case DataContractSource.Request:
    //             break;
    //         case DataContractSource.Trace:
    //             break;
    //         case DataContractSource.All:
    //             break;
    //         default:
    //             throw new ArgumentOutOfRangeException(nameof(source), source, null);
    //     }
    // }
    
    // public static AzureDataExplorerQuery<T> BuildQueryWithCustomDate<T>(
    //     Abstractions.Common.DataContractSource source, DateTimeOffset dateTimeOffset, int? topRows,
    //     bool? orderByTimestampDesc) where T : IDataContract
    // {
    //     var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
    //     parameterData.SetupQueryForCustom(dateTimeOffset);
    //     
    //     var result = new AzureDataExplorerQuery<T>()
    //     {
    //         QueryParameterData = parameterData
    //     };
    //
    //     return result;
    // }
    //
    // public static AzureDataExplorerQuery<T> BuildQueryWithTimeSpan<T>(
    //     Abstractions.Common.DataContractSource source, TimeSpan agoTimespan, int? topRows,
    //     bool? orderByTimestampDesc) where T : IDataContract
    // {
    //     var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
    //     parameterData.SetupQueryForTimespan(agoTimespan);
    //     
    //     var result = new AzureDataExplorerQuery<T>()
    //     {
    //         QueryParameterData = parameterData
    //     };
    //
    //     return result;
    // }
    //
    // public static AzureDataExplorerQuery<T> BuildQueryWithInterval<T>(
    //     Abstractions.Common.DataContractSource source, int interval, AgoPeriod agoPeriod, int? topRows,
    //     bool? orderByTimestampDesc) where T : IDataContract
    // {
    //     
    //     
    //     
    //     
    //     
    //     var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
    //     parameterData.SetupQueryForInterval(interval, agoPeriod);
    //     
    //     var result = new AzureDataExplorerQuery<T>()
    //     {
    //         QueryParameterData = parameterData
    //     };
    //
    //     return result;
    // }
    
    public static AzureDataExplorerQuery<T> BuildCustomQuery<T>(
        string tableName, string alias, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : IDataContract
    {
        var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
        var result = new AzureDataExplorerQuery<T>()
        {
            QueryParameterData = parameterData
        };
        return result;
    }
    
    public static AzureDataExplorerQuery<T> BuildCustomQueryWithCustomDate<T>(
        string tableName, string alias, DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : IDataContract
    {
        var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
        parameterData.SetupQueryForCustom(dateTimeOffset);
        
        var result = new AzureDataExplorerQuery<T>()
        {
            QueryParameterData = parameterData
        };

        return result;
    }
    
    public static AzureDataExplorerQuery<T> BuildCustomQueryWithTimeSpan<T>(
        string tableName, string alias, TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : IDataContract
    {
        var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
        parameterData.SetupQueryForTimespan(agoTimespan);
        
        var result = new AzureDataExplorerQuery<T>()
        {
            QueryParameterData = parameterData
        };

        return result;
    }
    
    public static AzureDataExplorerQuery<T> BuildCustomQueryWithInterval<T>(
        string tableName, string alias, int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : IDataContract
    {
        var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
        parameterData.SetupQueryForInterval(interval, agoPeriod);
        
        var result = new AzureDataExplorerQuery<T>()
        {
            QueryParameterData = parameterData
        };

        return result;
    }
    #endregion
    
    
    
    
}