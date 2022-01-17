using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Data.AzureDataExplorer.Queries;

/// <summary>
/// Known structure that is passed between the Query object and Telemetry Loader object 
/// </summary>
public class AzureDataExplorerQueryParameter
{

    #region Members

    /// <summary>
    /// Gets or sets the Kusto Query table name
    /// </summary>
    public string TableName { get; set; } = "";

    /// <summary>
    /// Gets or sets the Kusto Query Alias
    /// </summary>
    public string Alias { get; set; } = "";

    /// <summary>
    /// Gets or sets the interval to search data from. This works with the Ago Period <see cref="AgoPeriod"/>
    /// </summary>
    public int? AgoInterval { get; set; }

    /// <summary>
    /// Gets or sets the period to start looking for data
    /// </summary>
    public Abstractions.Common.AgoPeriod AgoPeriod { get; set; } = AgoPeriod.None;

    /// <summary>
    /// Gets or sets a timespan to start searching for data from
    /// </summary>
    public TimeSpan? AgoTimeSpan { get; set; }

    /// <summary>
    /// Gets or sets a fixed UTC date time to search data from
    /// </summary>
    public DateTimeOffset? AgoDateTime { get; set; }

    /// <summary>
    /// Gets or sets a value to indicate if the results should be ordered by Time Stamp Desc
    /// </summary>
    public bool? OrderByTimestampDesc { get; set; }

    /// <summary>
    /// The maximum number of rows to return from the query
    /// </summary>
    public int? TopRows { get; set; }

    #endregion

    #region ctor

    public AzureDataExplorerQueryParameter(
        string tableName, string alias, bool? orderByTimestampDesc = default, int? topRows = default)
    {
        if (string.IsNullOrEmpty(tableName) || tableName.Trim() == "")
            throw new ArgumentNullException(nameof(tableName));

        this.TableName = tableName;
        this.Alias = string.IsNullOrEmpty(alias) ? this.TableName : alias;
        this.OrderByTimestampDesc = orderByTimestampDesc;
        this.TopRows = topRows;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Helper method to setup the query by a Timespan
    /// </summary>
    /// <param name="agoTimespan">The timespan to start fetching data from</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetupQueryForTimespan(TimeSpan agoTimespan)
    {
        if (agoTimespan == null)
            throw new ArgumentNullException(nameof(agoTimespan));

        if (agoTimespan.Ticks < 0)
            throw new ArgumentOutOfRangeException(nameof(agoTimespan));

        this.AgoTimeSpan = agoTimespan;
        this.AgoPeriod = Abstractions.Common.AgoPeriod.Time;
    }

    /// <summary>
    /// Helper method to setup the query by a specified date to start fetching data from
    /// </summary>
    /// <param name="agoDateTime">The UTC date</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void SetupQueryForCustom(DateTimeOffset agoDateTime)
    {
        if (agoDateTime == null)
            throw new ArgumentNullException(nameof(AgoDateTime));

        this.AgoDateTime = agoDateTime;
        this.AgoPeriod = Abstractions.Common.AgoPeriod.Custom;
    }

    /// <summary>
    /// Helper method to setup the query by an interval
    /// </summary>
    /// <param name="agoInterval">The interval</param>
    /// <param name="agoPeriod">The type of interval</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="Exception"></exception>
    public void SetupQueryForInterval(int agoInterval, Abstractions.Common.AgoPeriod agoPeriod)
    {
        if (agoInterval < 0)
            throw new ArgumentOutOfRangeException(nameof(agoInterval));

        this.AgoInterval = agoInterval;

        if (agoPeriod == Abstractions.Common.AgoPeriod.Time)
            throw new Exception($"Use {nameof(SetupQueryForTimespan)} to setup a query for a timespan");

        if (agoPeriod == Abstractions.Common.AgoPeriod.Custom)
            throw new Exception($"Use {nameof(SetupQueryForCustom)} to setup a query for a timespan");

        if (agoPeriod == Abstractions.Common.AgoPeriod.None)
            throw new Exception($"Do not use {nameof(SetupQueryForInterval)} when no interval is required");

        this.AgoPeriod = agoPeriod;
    }

    /// <summary>
    /// Takes all the options specified and builds a Kusto Query that can be used by the telemetry loader
    /// </summary>
    /// <returns></returns>
    public string BuildKustoQuery()
    {
        var outputLines = new List<string>();

        outputLines.Add($"{TableName}");

        if (this.AgoPeriod != AgoPeriod.None)
        {
            if (this.AgoPeriod == AgoPeriod.Custom)
            {
                outputLines.Add($"where timestamp > datetime({GetDateString(AgoDateTime.Value)})");
            }
            else if (this.AgoPeriod == AgoPeriod.Time)
            {
                outputLines.Add($"where timestamp > ago(time({GetTimeString(AgoTimeSpan.Value)}))");
            }
            else
            {
                outputLines.Add($"where timestamp > ago({GetAgoString(this.AgoInterval.Value, this.AgoPeriod)})");
            }
        }

        if (OrderByTimestampDesc.HasValue)
        {
            if (OrderByTimestampDesc.Value == false)
                outputLines.Add("sort by timestamp asc nulls first");
            else
                outputLines.Add("sort by timestamp desc nulls last");
        }

        if (TopRows.HasValue)
        {
            outputLines.Add($"take {TopRows.Value}");
        }

        if (!string.IsNullOrEmpty(Alias))
        {
            outputLines.Add($"as {Alias}");
        }

        return string.Join(" | ", outputLines);
    }

    #endregion

    #region Internal Methods

    internal string GetDateString(DateTimeOffset dateTimeOffset)
    {
        var utcDateTime = dateTimeOffset.ToUniversalTime();
        return utcDateTime.ToString(@"yyyy-MM-dd HH:mm:ss.fff");
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