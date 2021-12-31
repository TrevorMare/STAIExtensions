using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Core.Tests.Fixtures;
using Xunit;

namespace STAIExtensions.Core.Tests.DataSet;

public class DataSetTests
{

    private IMediator SetupDI()
    {
        var di = new ServiceCollection();
        var mediatR = Substitute.For<IMediator>();
        di.AddSingleton<IMediator>(mediatR);
        Core.StartupExtensions.UseSTAIExtensions(di);
        return mediatR;
    }
    
    
    
    [Fact]
    public void ctor_WhenTelemetryLoaderIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new DataSetFixture(null, ""));
    }

    [Fact]
    public void ctor_WhenDataSetNameIsNull_ShouldGenerateName()
    {
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var sut = new DataSetFixture(telemetryLoader, null);
        
        Assert.NotNull(sut.DataSetName);
    }

    [Fact]
    public void ctor_WhenConstructed_ShouldGenerateId()
    {
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var sut = new DataSetFixture(telemetryLoader, null);
        
        Assert.NotNull(sut.DataSetId);
    }
    
    [Fact]
    public void StartAutoRefresh_WhenTimeoutIsLessThan0_ShouldThrowArgumentException()
    {
        
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var sut = new DataSetFixture(telemetryLoader, null);

        Assert.Throws<ArgumentException>(() => sut.StartAutoRefresh(TimeSpan.FromDays(-1)));
    }
    
    [Fact]
    public void ExecuteDataQuery_WhenQueryIsNull_ShouldThrowArgumentNullException()
    {
        
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var sut = new DataSetFixture(telemetryLoader, null);
        FixtureDataContractQuery<Abstractions.DataContracts.Models.Availability> query = null;

        Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteDataQueryProxy(query));
    }
    
}