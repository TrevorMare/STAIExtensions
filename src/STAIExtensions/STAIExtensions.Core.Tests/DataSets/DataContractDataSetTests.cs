using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.Extensions;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Core.DataSets;
using STAIExtensions.Core.DataSets.Options;
using STAIExtensions.Core.Tests.Fixtures;
using Xunit;

namespace STAIExtensions.Core.Tests.DataSets;

public class DataContractDataSetTests
{

    private IDataContractQueryFactory BuildQueryFactorySubstitute()
    {
        var queryFactory = Substitute.For<IDataContractQueryFactory>();

        queryFactory.BuildAvailabilityQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<Availability>());
        queryFactory.BuildBrowserTimingQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<BrowserTiming>());
        queryFactory.BuildCustomEventQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<CustomEvent>());
        queryFactory.BuildCustomMetricQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<CustomMetric>());
        queryFactory.BuildDependencyQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<Dependency>());
        queryFactory.BuildExceptionQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<AIException>());
        queryFactory.BuildPageViewQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<PageView>());
        queryFactory.BuildPerformanceCounterQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<PerformanceCounter>());
        queryFactory.BuildRequestQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<Request>());
        queryFactory.BuildTraceQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<Trace>());
        
        
        queryFactory.BuildAvailabilityQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<Availability>());
        queryFactory.BuildBrowserTimingQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<BrowserTiming>());
        queryFactory.BuildCustomEventQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<CustomEvent>());
        queryFactory.BuildCustomMetricQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<CustomMetric>());
        queryFactory.BuildDependencyQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<Dependency>());
        queryFactory.BuildExceptionQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<AIException>());
        queryFactory.BuildPageViewQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<PageView>());
        queryFactory.BuildPerformanceCounterQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<PerformanceCounter>());
        queryFactory.BuildRequestQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<Request>());
        queryFactory.BuildTraceQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<Trace>());
        return queryFactory;
    }

    private void BuildTelemetryLoaderResultSets(ITelemetryLoader telemetryLoader)
    {

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<Availability>>())
            .Returns(new List<Availability>() { new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<BrowserTiming>>())
            .Returns(new List<BrowserTiming>() { new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<CustomEvent>>())
            .Returns(new List<CustomEvent>() { new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<CustomMetric>>())
            .Returns(new List<CustomMetric>() {new(), new()});
        
        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<Dependency>>())
            .Returns(new List<Dependency>() {new(), new()});
        
        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<AIException>>())
            .Returns(new List<AIException>() {new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<PageView>>())
            .Returns(new List<PageView>() {new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<PerformanceCounter>>())
            .Returns(new List<PerformanceCounter>() {new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<Request>>())
            .Returns(new List<Request>() {new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<Trace>>())
            .Returns(new List<Trace>() {new(), new()});
    }
    
    private ITelemetryLoader BuildTelemetryLoader()
    {

        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var queryFactory = BuildQueryFactorySubstitute();
        telemetryLoader.DataContractQueryFactory.Returns(queryFactory);
        BuildTelemetryLoaderResultSets(telemetryLoader);
        return telemetryLoader;
    }

    [Fact]
    public void DataContractDataSetOptions_WhenConstructed_ShouldBeEnabledByDefault()
    {
        var sut = new DataContractDataSetOptions();
        Assert.True(sut.Availiblity.Enabled);
    }
    
    [Fact]
    public void DataContractDataSetOptions_WhenConstructed_ShouldHaveBufferSetByDefault()
    {
        var sut = new DataContractDataSetOptions();
        Assert.NotNull(sut.Availiblity.BufferSize);
    }

    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddAvailability()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.Availability.Any());
    }
    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddBrowserTimings()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.BrowserTiming.Any());
    }
    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddCustomEvents()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.CustomEvents.Any());
    }
    
    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddCustomMetrics()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.CustomMetrics.Any());
    }
    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddDependencies()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.Dependencies.Any());
    }
    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddExceptions()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.Exceptions.Any());
    }
    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddPageViews()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.PageViews.Any());
    }
    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddPerformanceCounters()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.PerformanceCounters.Any());
    }
    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddRequests()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.Requests.Any());
    }
    
    [Fact]
    public async Task UpdateDataSet_WhenCalled_ShouldAddTraces()
    {
        var telemetryLoader = BuildTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        await sut.UpdateDataSet();
        
        Assert.True(sut.Traces.Any());
    }
    
    [Fact]
    public async Task UpdateDataSet_WhenCompleted_ShouldRaiseEvent()
    {
        var telemetryLoader = BuildTelemetryLoader();
        int eventCounter = 0;

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");
        sut.OnDataSetUpdated += (sender, args) => { eventCounter++; }; 

        await sut.UpdateDataSet();
        
        Assert.Equal(1, eventCounter);
    }
    
    
}