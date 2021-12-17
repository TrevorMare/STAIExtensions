using System.ComponentModel;
using STAIExtensions.Abstractions.ApiClient.Models;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Serialization;

namespace STAIExtensions.Core.Queries.Models;

public class DataContractQuery : Abstractions.Queries.IDataContractQuery
{
    #region Properties
    public Func<ITableRowDeserializer, ApiClientQueryResultTable,
        IEnumerable<Abstractions.DataContracts.IKustoQueryContract>>? DataRowDeserializer { get; set; }

    public object? Tag { get; set; }

    public string DeserializedTableName { get; set; }

    public string? TableName { get; set; } = "";

    public string? Alias { get; set; } = "";

    public int? TopRows { get; set; } = null;

    public int? AgoInterval { get; set; } = null;

    public Abstractions.Common.AgoPeriod? AgoPeriod { get; set; } = null;

    public TimeSpan? AgoTimeSpan { get; set; }

    public bool? OrderByTimestampAsc { get; set; }

    public bool Enabled { get; set; } = true;

    #endregion

    #region ctor

    public DataContractQuery()
    {
    }

    public DataContractQuery(string tableName, string alias, int? agoInterval = default,
        Abstractions.Common.AgoPeriod? agoPeriod = default, int? topRows = default, bool? orderByTimestampAsc = default)
        : this(tableName, alias, agoInterval, agoPeriod, null, topRows, orderByTimestampAsc)
    {
    }

    public DataContractQuery(Abstractions.Common.AzureApiDataContractSource dataContractSource,
        int? agoInterval = default, Abstractions.Common.AgoPeriod? agoPeriod = default, int? topRows = default,
        bool? orderByTimestampAsc = default)
        : this(GetTableNameFromAzureSource(dataContractSource), null, agoInterval, agoPeriod, null, topRows,
            orderByTimestampAsc)
    {
    }

    public DataContractQuery(string tableName, string alias, TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampAsc = default)
        : this(tableName, alias, null, null, agoTimespan, topRows, orderByTimestampAsc)
    {
    }

    public DataContractQuery(Abstractions.Common.AzureApiDataContractSource dataContractSource, TimeSpan agoTimespan,
        int? topRows = default, bool? orderByTimestampAsc = default)
        : this(GetTableNameFromAzureSource(dataContractSource), null, null, null, agoTimespan, topRows,
            orderByTimestampAsc)
    {
    }

    private DataContractQuery(string tableName, string? alias = default, int? agoInterval = default,
        Abstractions.Common.AgoPeriod? agoPeriod = default, TimeSpan? agoTimespan = default, int? topRows = default,
        bool? orderByTimestampAsc = default)
    {
        if (string.IsNullOrEmpty(tableName))
            throw new ArgumentNullException(nameof(tableName));

        this.TableName = tableName;

        this.Alias = string.IsNullOrEmpty(alias) ? this.TableName : alias;
        this.DeserializedTableName = this.Alias ?? "table";

        this.AgoInterval = agoInterval;
        this.AgoPeriod = agoPeriod;
        this.AgoTimeSpan = agoTimespan;
        this.TopRows = topRows;
        this.OrderByTimestampAsc = orderByTimestampAsc;

        // If the ago timespan is passed, the period should be set to time regardless
        if (agoTimespan.HasValue)
        {
            this.AgoPeriod = Abstractions.Common.AgoPeriod.Time;
        }
        else
        {
            if (agoPeriod.HasValue == false)
            {
                throw new ArgumentNullException(nameof(agoPeriod));
            }

            if (agoInterval.HasValue == false)
            {
                throw new ArgumentNullException(nameof(agoInterval));
            }

            // Validate the ago period, it's not allowed to be time here as this is an interval
            if (agoPeriod == Abstractions.Common.AgoPeriod.Time)
            {
                throw new InvalidEnumArgumentException("Time ago period is only allowed for a timespan.");
            }
        }
    }

    #endregion

    #region Methods

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
                    queryBuilderItems.Add($"where timestamp >= ago(time({GetTimeString(AgoTimeSpan.Value)}))");
                }
            }
            else
            {
                if (AgoInterval.HasValue)
                {
                    queryBuilderItems.Add(
                        $"where timestamp >= ago({GetAgoString(this.AgoInterval.Value, this.AgoPeriod.Value)})");
                }
            }
        }

        if (OrderByTimestampAsc.HasValue)
        {
            if (OrderByTimestampAsc.Value == true)
                queryBuilderItems.Add("sort by timestamp asc nulls first");
            else
                queryBuilderItems.Add("sort by timestamp desc nulls last");
        }

        if (TopRows.HasValue)
        {
            queryBuilderItems.Add($"take {TopRows.Value}");
        }

        if (!string.IsNullOrEmpty(Alias))
        {
            queryBuilderItems.Add($"as {Alias}");
        }

        return string.Join(" | ", queryBuilderItems) + ";";
    }

    public virtual string BuildKustoQuery()
    {
        return ToString();
    }

    #endregion

    #region Helper Methods

    private static string GetTableNameFromAzureSource(Abstractions.Common.AzureApiDataContractSource value)
    {
        if (value == AzureApiDataContractSource.All)
            throw new InvalidEnumArgumentException("Query builder data source option All is not valid");

        return value.DisplayName();
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