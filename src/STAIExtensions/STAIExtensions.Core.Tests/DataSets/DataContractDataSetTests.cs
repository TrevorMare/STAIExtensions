using System.Linq;
using NSubstitute;
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

    private ITelemetryLoader GetTelemetryLoader()
    {

        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var queryFactory = Substitute.For<IDataContractQueryFactory>();

        queryFactory.BuildAvailabilityQuery().Returns(new FixtureDataContractQuery<IAvailability>());

        telemetryLoader.DataContractQueryFactory.Returns(queryFactory);

        return telemetryLoader;
    }

    [Fact]
    public void DataContractDataSetOptions_WhenConstructed_ShouldHaveDefaultValues()
    {
        var sut = new DataContractDataSetOptions();
        
        Assert.True(sut.Availiblity.Enabled);
    }
    

    [Fact]
    public void Scratch()
    {
        var telemetryLoader = GetTelemetryLoader();

        var sut = new DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");

        var actual = sut.GetQueriesToExecute().ToList();
        
        Assert.NotNull(actual);
        
        Assert.True(actual.Any());

    }
    
    
}