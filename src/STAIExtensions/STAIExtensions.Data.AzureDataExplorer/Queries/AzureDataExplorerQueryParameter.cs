using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Data.AzureDataExplorer.Queries;

public class AzureDataExplorerQueryParameter
{

    #region Members

    public string TableName { get; set; } = "";

    public string Alias { get; set; } = "";

    public int? AgoInterval { get; set; }

    public Abstractions.Common.AgoPeriod AgoPeriod { get; set; } = AgoPeriod.None;

    public TimeSpan? AgoTimeSpan { get; set; }

    public DateTimeOffset? AgoDateTime { get; set; }

    public bool? OrderByTimestampDesc { get; set; }

    public int? TopRows { get; set; }

    #endregion

    #region ctor

    public AzureDataExplorerQueryParameter(
        string tableName, string alias, bool? orderByTimestampDesc = default, int? topRows = default)
    {
        if (string.IsNullOrEmpty(tableName))
            throw new ArgumentNullException(nameof(tableName));

        this.TableName = tableName;
        this.Alias = string.IsNullOrEmpty(alias) ? this.TableName : alias;
        this.OrderByTimestampDesc = orderByTimestampDesc;
        this.TopRows = topRows;
    }

    #endregion

    #region Methods

    public void SetupQueryForTimespan(TimeSpan agoTimespan)
    {
        if (agoTimespan == null)
            throw new ArgumentNullException(nameof(agoTimespan));

        if (agoTimespan.Ticks < 0)
            throw new ArgumentOutOfRangeException(nameof(agoTimespan));

        this.AgoTimeSpan = agoTimespan;
        this.AgoPeriod = Abstractions.Common.AgoPeriod.Time;
    }

    public void SetupQueryForCustom(DateTimeOffset agoDateTime)
    {
        if (AgoDateTime == null)
            throw new ArgumentNullException(nameof(AgoDateTime));

        this.AgoDateTime = AgoDateTime;
        this.AgoPeriod = Abstractions.Common.AgoPeriod.Custom;
    }

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

    public string BuildKustoQuery()
    {
        var outputLines = new List<string>();

        outputLines.Add($"{TableName}");

        if (this.AgoPeriod != AgoPeriod.None)
        {
            if (this.AgoPeriod == AgoPeriod.Custom)
            {
                outputLines.Add($"where timestamp >= datetime({GetDateString(AgoDateTime.Value)})");
            }
            else if (this.AgoPeriod == AgoPeriod.Time)
            {
                outputLines.Add($"where timestamp >= ago(time({GetTimeString(AgoTimeSpan.Value)}))");
            }
            else
            {
                outputLines.Add($"where timestamp >= ago({GetAgoString(this.AgoInterval.Value, this.AgoPeriod)})");
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
        return dateTimeOffset.ToString(@"yyyy-MM-dd HH:mm:ss");
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