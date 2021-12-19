using System;
using System.ComponentModel;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Data.AzureDataExplorer.Queries;
using Xunit;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Queries;

public class AzureDataExplorerQueryParameterTests
{
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ctor_WhenTableNameIsNull_ShouldThrowException(string tableName)
    {
        Assert.Throws<ArgumentNullException>(() =>
            new AzureDataExplorerQueryParameter(tableName: tableName, alias: null)); 
    }
    
    [Fact]
    public void ctor_WhenConstructed_ShouldSetProperties()
    {
        var sut = new AzureDataExplorerQueryParameter("table", null, false, 3);
        
        Assert.True(!string.IsNullOrEmpty(sut.TableName));
        Assert.True(!string.IsNullOrEmpty(sut.Alias));
        Assert.NotNull(sut.OrderByTimestampDesc);
        Assert.NotNull(sut.TopRows);
    }
    
    [Fact]
    public void SetupQueryForTimespan_WhenIntervalIsLessThan0_ShouldThrowException()
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.SetupQueryForTimespan(TimeSpan.FromTicks(-200)));
    }
    
    [Fact]
    public void SetupQueryForTimespan_WhenIntervalIsSet_ShouldSetParameters()
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        sut.SetupQueryForTimespan(TimeSpan.FromTicks(200));
        
        Assert.NotNull(sut.AgoTimeSpan);
        Assert.True(sut.AgoPeriod == AgoPeriod.Time);
    }
    
    [Fact]
    public void SetupQueryForCustom_WhenParameterIsSet_ShouldSetParameters()
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        sut.SetupQueryForCustom(DateTimeOffset.Now);
        
        Assert.NotNull(sut.AgoDateTime);
        Assert.True(sut.AgoPeriod == AgoPeriod.Custom);
    }
    
    [Fact]
    public void SetupQueryForInterval_WhenIntervalIsLessThan0_ShouldThrowException()
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.SetupQueryForInterval(-2, AgoPeriod.Days));
    }
    
    [Fact]
    public void SetupQueryForInterval_WhenPeriodIsCustom_ShouldThrowException()
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        Assert.Throws<Exception>(() => sut.SetupQueryForInterval(2, AgoPeriod.Custom));
    }
    
    [Fact]
    public void SetupQueryForInterval_WhenPeriodIsTime_ShouldThrowException()
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        Assert.Throws<Exception>(() => sut.SetupQueryForInterval(2, AgoPeriod.Time));
    }
    
    [Fact]
    public void SetupQueryForInterval_WhenPeriodIsNone_ShouldThrowException()
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        Assert.Throws<Exception>(() => sut.SetupQueryForInterval(2, AgoPeriod.None));
    }
    
    [Fact]
    public void SetupQueryForInterval_WhenParametersIsValid_ShouldSetParameters()
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        sut.SetupQueryForInterval(2, AgoPeriod.Days);
        
        Assert.True(sut.AgoInterval == 2);
        Assert.True(sut.AgoPeriod == AgoPeriod.Days);
    }
    
    [Fact]
    public void ctor_WhenConstructedWithNoAlias_ShouldUseTableName()
    {
        var sut = new AzureDataExplorerQueryParameter("table", null, false, 3);
        Assert.Equal("table", sut.Alias);
    }
    
    [Fact]
    public void ctor_WhenConstructedWithAlias_ShouldUseAliasName()
    {
        var sut = new AzureDataExplorerQueryParameter("table", "alias", false, 3);
        Assert.Equal("alias", sut.Alias);
    }
    
    [Theory]
    [InlineData(1, AgoPeriod.Days, "1d")]
    [InlineData(1, AgoPeriod.Hours, "1h")]
    [InlineData(1, AgoPeriod.Microseconds, "1microsecond")]
    [InlineData(1, AgoPeriod.Milliseconds, "1ms")]
    [InlineData(1, AgoPeriod.Minutes, "1m")]
    [InlineData(1, AgoPeriod.Nanoseconds, "1tick")]
    [InlineData(1, AgoPeriod.Seconds, "1s")]
    public void GetAgoString_WhenParametersPassed_ShouldGenerateApplicableString(int interval, AgoPeriod agoPeriod, string expected)
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        var actual = sut.GetAgoString(interval, agoPeriod);
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void GetTimeString_WhenParametersPassed_ShouldGenerateApplicableString()
    {
        var sut = new AzureDataExplorerQueryParameter("tableName", "alias");
        var interval = TimeSpan.FromTicks(452967000000);
        
        var actual = sut.GetTimeString(interval);
        
        Assert.Equal("00.12:34:56", actual);
    }
    
    [Theory]
    [InlineData("table1", null, null, "table1 | where timestamp >= ago(1d) | as table1")]
    [InlineData("table1", 1, null, "table1 | where timestamp >= ago(1d) | take 1 | as table1")]
    [InlineData("table1", null, false, "table1 | where timestamp >= ago(1d) | sort by timestamp asc nulls first | as table1")]
    [InlineData("table1", 1, true, "table1 | where timestamp >= ago(1d) | sort by timestamp desc nulls last | take 1 | as table1")]
    public void BuildKustoQuery_WhenIntervalParametersPassed_ShouldGenerateQuery(string tableName, int? topRows, bool? orderByTimestampDesc, string expected)
    {
        var sut = new AzureDataExplorerQueryParameter(tableName, null, orderByTimestampDesc, topRows);
        sut.SetupQueryForInterval(1, AgoPeriod.Days);
        
        var actual = sut.BuildKustoQuery();
    
        Assert.Equal(expected, actual);
    }
    
}