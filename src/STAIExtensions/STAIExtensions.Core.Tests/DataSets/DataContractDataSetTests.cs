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

        queryFactory.BuildAvailabilityQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IAvailability>());
        queryFactory.BuildBrowserTimingQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IBrowserTiming>());
        queryFactory.BuildCustomEventQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<ICustomEvent>());
        queryFactory.BuildCustomMetricQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<ICustomMetrics>());
        queryFactory.BuildDependencyQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IDependency>());
        queryFactory.BuildExceptionQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IException>());
        queryFactory.BuildPageViewQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IPageView>());
        queryFactory.BuildPerformanceCounterQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IPerformanceCounter>());
        queryFactory.BuildRequestQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IRequest>());
        queryFactory.BuildTraceQuery(Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<ITrace>());
        
        
        queryFactory.BuildAvailabilityQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IAvailability>());
        queryFactory.BuildBrowserTimingQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IBrowserTiming>());
        queryFactory.BuildCustomEventQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<ICustomEvent>());
        queryFactory.BuildCustomMetricQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<ICustomMetrics>());
        queryFactory.BuildDependencyQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IDependency>());
        queryFactory.BuildExceptionQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IException>());
        queryFactory.BuildPageViewQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IPageView>());
        queryFactory.BuildPerformanceCounterQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IPerformanceCounter>());
        queryFactory.BuildRequestQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<IRequest>());
        queryFactory.BuildTraceQueryWithCustomDate(Arg.Any<DateTimeOffset>(),Arg.Any<int>(), Arg.Any<bool>()).Returns(new FixtureDataContractQuery<ITrace>());
        return queryFactory;
    }

    private void BuildTelemetryLoaderResultSets(ITelemetryLoader telemetryLoader)
    {

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<IAvailability>>())
            .Returns(new List<FixtureAvailability>() { new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<IBrowserTiming>>())
            .Returns(new List<FixtureBrowserTiming>() { new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<ICustomEvent>>())
            .Returns(new List<FixtureCustomEvent>() { new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<ICustomMetrics>>())
            .Returns(new List<FixtureCustomMetrics>() {new(), new()});
        
        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<IDependency>>())
            .Returns(new List<FixtureDependency>() {new(), new()});
        
        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<IException>>())
            .Returns(new List<FixtureException>() {new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<IPageView>>())
            .Returns(new List<FixturePageView>() {new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<IPerformanceCounter>>())
            .Returns(new List<FixturePerformanceCounter>() {new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<IRequest>>())
            .Returns(new List<FixtureRequest>() {new(), new()});

        telemetryLoader.ExecuteQueryAsync(Arg.Any<DataContractQuery<ITrace>>())
            .Returns(new List<FixtureTrace>() {new(), new()});
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