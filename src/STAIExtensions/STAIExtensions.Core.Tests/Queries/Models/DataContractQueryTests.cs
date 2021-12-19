using System;
using System.ComponentModel;
using STAIExtensions.Abstractions.Common;
using Xunit;

namespace STAIExtensions.Core.Tests.Queries.Models;

public class DataContractQueryTests
{
    //
    // [Fact]
    // public void ctor_WhenNoParametersPassed_ShouldNotFail()
    // {
    //     var sut = new DataContractQuery();
    //     Assert.NotNull(sut);
    // }
    //
    // [Fact]
    // public void ctor_WhenTableNameIsNull_ShouldThrowException()
    // {
    //     Assert.Throws<ArgumentNullException>(() =>
    //         new DataContractQuery(tableName: null, alias: null, agoTimespan : TimeSpan.Zero, topRows: null)); 
    // }
    //
    // [Fact]
    // public void ctor_WhenDataSourceIsAll_ShouldThrowException()
    // {
    //     Assert.Throws<InvalidEnumArgumentException>(() =>
    //         new DataContractQuery(Abstractions.Common.AzureApiDataContractSource.All, agoTimespan: TimeSpan.Zero, topRows: null)); 
    // }
    //
    // [Fact]
    // public void ctor_WhenConstructedAsTimespan_ShouldSetProperties()
    // {
    //     var sut = new DataContractQuery(Abstractions.Common.AzureApiDataContractSource.Availability, agoTimespan: TimeSpan.FromDays(2), topRows: 2);
    //     
    //     Assert.True(!string.IsNullOrEmpty(sut.TableName));
    //     Assert.NotNull(sut.AgoTimeSpan);
    //     Assert.NotNull(sut.TopRows);
    // }
    //
    // [Fact]
    // public void ctor_WhenConstructedAsInterval_ShouldSetProperties()
    // {
    //     var sut = new DataContractQuery(Abstractions.Common.AzureApiDataContractSource.Availability, agoInterval: 2, AgoPeriod.Days, topRows: 2, orderByTimestampAsc: true);
    //     
    //     Assert.True(!string.IsNullOrEmpty(sut.TableName));
    //     Assert.NotNull(sut.AgoInterval);
    //     Assert.NotNull(sut.AgoPeriod);
    //     Assert.NotNull(sut.TopRows);
    //     Assert.NotNull(sut.OrderByTimestampAsc);
    // }
    //
    // [Fact]
    // public void ctor_WhenConstructedWithNoAlias_ShouldUseTableName()
    // {
    //     var sut = new DataContractQuery(tableName: "tablename", alias: "", agoInterval: 2, AgoPeriod.Days, topRows: 2);
    //     Assert.Equal("tablename", sut.Alias);
    // }
    //
    // [Fact]
    // public void ctor_WhenConstructedWithAlias_ShouldUseAliasName()
    // {
    //     var sut = new DataContractQuery(tableName: "tablename", alias: "alias", agoInterval: 2, AgoPeriod.Days, topRows: 2);
    //     Assert.Equal("alias", sut.Alias);
    // }
    //
    // [Fact]
    // public void ctor_WhenConstructedWithTimespan_AgoPeriodShouldDefaultToTime()
    // {
    //     var sut = new DataContractQuery(tableName: "tablename", alias: "alias", agoTimespan: TimeSpan.Zero, topRows: 2);
    //     Assert.Equal(AgoPeriod.Time, sut.AgoPeriod);
    // }
    //
    // [Fact]
    // public void ctor_WhenConstructedWithIntervalAndNoPeriod_ShouldThrowException()
    // {
    //     Assert.Throws<ArgumentNullException>(() =>
    //         new DataContractQuery(tableName: "tablename", alias: "alias", agoInterval: 1, agoPeriod: null, topRows: 2)); 
    // }
    //
    // [Fact]
    // public void ctor_WhenConstructedWithNoIntervalAndPeriod_ShouldThrowException()
    // {
    //     Assert.Throws<ArgumentNullException>(() =>
    //         new DataContractQuery(tableName: "tablename", alias: "alias", agoInterval: null, agoPeriod: AgoPeriod.Days, topRows: 2)); 
    // }
    //
    // [Theory]
    // [InlineData(1, AgoPeriod.Days, "1d")]
    // [InlineData(1, AgoPeriod.Hours, "1h")]
    // [InlineData(1, AgoPeriod.Microseconds, "1microsecond")]
    // [InlineData(1, AgoPeriod.Milliseconds, "1ms")]
    // [InlineData(1, AgoPeriod.Minutes, "1m")]
    // [InlineData(1, AgoPeriod.Nanoseconds, "1tick")]
    // [InlineData(1, AgoPeriod.Seconds, "1s")]
    // public void GetAgoString_WhenParametersPassed_ShouldGenerateApplicableString(int interval, AgoPeriod agoPeriod, string expected)
    // {
    //     var sut = new DataContractQuery();
    //     var actual = sut.GetAgoString(interval, agoPeriod);
    //     
    //     Assert.Equal(expected, actual);
    // }
    //
    // [Fact]
    // public void GetTimeString_WhenParametersPassed_ShouldGenerateApplicableString()
    // {
    //     var sut = new DataContractQuery();
    //     var interval = TimeSpan.FromTicks(452967000000);
    //     
    //     var actual = sut.GetTimeString(interval);
    //     
    //     Assert.Equal("00.12:34:56", actual);
    // }
    //
    // [Theory]
    // [InlineData("table1", null, null, "table1 | where timestamp >= ago(1d) | as table1;")]
    // [InlineData("table1", 1, null, "table1 | where timestamp >= ago(1d) | take 1 | as table1;")]
    // [InlineData("table1", null, true, "table1 | where timestamp >= ago(1d) | sort by timestamp asc nulls first | as table1;")]
    // [InlineData("table1", 1, true, "table1 | where timestamp >= ago(1d) | sort by timestamp asc nulls first | take 1 | as table1;")]
    // public void BuildKustoQuery_WhenIntervalParametersPassed_ShouldGenerateQuery(string tableName, int? topRows, bool? orderByTimestampDesc, string expected)
    // {
    //     var sut = new DataContractQuery(tableName, tableName, 1, AgoPeriod.Days, topRows, orderByTimestampDesc);
    //
    //     var actual = sut.BuildKustoQuery();
    //
    //     Assert.Equal(expected, actual);
    // }
    //
}