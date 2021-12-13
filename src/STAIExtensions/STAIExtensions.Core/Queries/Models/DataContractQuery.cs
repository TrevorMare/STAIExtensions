using System.ComponentModel;
using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Core.Queries.Models;

public class DataContractQuery : Abstractions.Queries.IDataContractQuery
{

    #region Properties

    public string? TableName { get; set; } = "";

    public string? Alias { get; set; } = "";

    public int? TopRows { get; set; } = null;

    public int? AgoInterval { get; set; } = null;

    public Abstractions.Common.AgoPeriod? AgoPeriod { get; set; } = null;

    public TimeSpan? AgoTimeSpan { get; set; }
    #endregion

    #region ctor
    public DataContractQuery()
    {
        
    }

    public DataContractQuery(string tableName, string alias, int? agoInterval = default, Abstractions.Common.AgoPeriod? agoPeriod = default, int? topRows = default) 
        : this(tableName, alias, agoInterval, agoPeriod, null, topRows)
    {
        
    }

    public DataContractQuery(Abstractions.Common.AzureApiDataContractSource dataContractSource, int? agoInterval = default, Abstractions.Common.AgoPeriod? agoPeriod = default, int? topRows = default)
        : this(GetTableNameFromAzureSource(dataContractSource), null, agoInterval, agoPeriod, null, topRows)
    {
        
    }
    
    public DataContractQuery(string tableName, string alias, TimeSpan agoTimespan, int? topRows = default)
        : this(tableName, alias, null, null, agoTimespan, topRows)
    {
        
    }

    public DataContractQuery(Abstractions.Common.AzureApiDataContractSource dataContractSource, TimeSpan agoTimespan, int? topRows = default)
        : this(GetTableNameFromAzureSource(dataContractSource), null, null, null, agoTimespan, topRows)
    {
        
    }

    internal DataContractQuery(string tableName, string? alias = default, int? agoInterval = default, Abstractions.Common.AgoPeriod? agoPeriod = default, TimeSpan? timespan = default, int? topRows = default)
    {
        if (string.IsNullOrEmpty(tableName))
            throw new ArgumentNullException(nameof(tableName));
        
        this.TableName = tableName;

        this.Alias = string.IsNullOrEmpty(alias) ? this.TableName : alias;
        this.AgoInterval = agoInterval;
        this.AgoPeriod = agoPeriod;
        this.AgoTimeSpan = timespan;
        this.TopRows = topRows;
        
        if (this.AgoTimeSpan != null)
        {
            this.AgoPeriod = Abstractions.Common.AgoPeriod.Time;
        }
        else
        {
            if (agoInterval.HasValue)
            {
                if (agoPeriod.HasValue == false)
                    throw new Exception("An ago period is required when an interval is specified.");
            }

            if (agoPeriod.HasValue && agoPeriod.Value != Abstractions.Common.AgoPeriod.Time)
            {
                if (agoInterval.HasValue == false)
                    throw new Exception("An ago interval is required when an ago period is specified.");
            }
        }
    }
    #endregion

    #region Methods
    private static string GetTableNameFromAzureSource(Abstractions.Common.AzureApiDataContractSource value)
    {
        if (value == AzureApiDataContractSource.All)
            throw new InvalidEnumArgumentException("Query builder data source option All is not valid");
        
        return value.DisplayName();
    }
    
    public override string ToString()
    {
        var queryBuilderItems = new List<string>();

        if (string.IsNullOrEmpty(this.TableName))
            throw new Exception("Table name is required");
        
        queryBuilderItems.Add(this.TableName);

        if (AgoPeriod.HasValue)
        {
            if (AgoPeriod.Value == Abstractions.Common.AgoPeriod.Time)
            {
                if (AgoTimeSpan != null)
                {
                    queryBuilderItems.Add($"where timespan >= ago(time({GetTimeString(AgoTimeSpan.Value)}))");
                }
            }
            else
            {
                if (AgoInterval.HasValue)
                {
                    queryBuilderItems.Add($"where timespan >= ago({GetAgoString(this.AgoInterval.Value, this.AgoPeriod.Value)})");
                }
            }
        }

        if (TopRows.HasValue)
        {
            queryBuilderItems.Add($"take {TopRows.Value}");
        }

        if (!string.IsNullOrEmpty(Alias))
        {
            queryBuilderItems.Add($"as {Alias}");
        }
        
        return string.Join(" | ", queryBuilderItems);
    }

    public string BuildKustoQuery()
    {
        return ToString();
    }

    internal string GetAgoString(int interval, Abstractions.Common.AgoPeriod agoPeriod)
    {
        return $"{interval}{agoPeriod.DisplayName()}";
    }
    
    internal string GetTimeString(TimeSpan inputTime)
    {
        return inputTime.ToString(@"dd\.hh\:mm\:ss");
    }
    #endregion
    
}