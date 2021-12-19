using System;
using System.Linq;
using System.Threading.Tasks;
using STAIExtensions.Data.AzureDataExplorer.Models;
using Xunit;

namespace STAIExtensions.Data.AzureDataExplorer.Tests;

public class AzureDataExplorerClientTests
{

    private TelemetryLoaderOptions options = new TelemetryLoaderOptions("abc", "abc"); 

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ConfigureApi_WhenAppIdNullOrEmpty_ShouldThrowException(string appId)
    {
        Assert.Throws<ArgumentNullException>(() => new AzureDataExplorerClient(new TelemetryLoaderOptions()
        {
            ApiKey = "Abc",
            AppId = appId
        }));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ConfigureApi_WhenAppKeyNullOrEmpty_ShouldThrowException(string appKey)
    {
        Assert.Throws<ArgumentNullException>(() => new AzureDataExplorerClient(new TelemetryLoaderOptions()
        {
            ApiKey = appKey,
            AppId = "Abc"
        }));
    }


    [Fact]
    public void ExecuteQuery_WhenErrorThrown_ShouldReturnResponseObject()
    {
        var sut = new AzureDataExplorerClient(options);
        
        var actual = sut.ExecuteQuery(null);
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void ExecuteQuery_WhenErrorThrown_ResultShouldContainErrorMessage()
    {
        var sut = new AzureDataExplorerClient(options);
        var actual = sut.ExecuteQuery(null);
        Assert.False(string.IsNullOrEmpty(actual?.ErrorMessage));
    }
    
    [Fact]
    public void ExecuteQuery_WhenErrorThrown_ResultSuccessShouldBeFalse()
    {
        var sut = new AzureDataExplorerClient(options);
        var actual = sut.ExecuteQuery(null);
        Assert.False(actual?.Success);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ExecuteQuery_WhenQueryIsEmptyOrNull_ResultShouldBeFalse(string query)
    {
        var sut = new AzureDataExplorerClient(options);
        
        var actual = sut.ExecuteQuery(query);
        Assert.False(actual.Success);
    }
    
    [Fact]
    public async Task ExecuteQueryAsync_WhenErrorThrown_ShouldReturnResponseObject()
    {
        var sut = new AzureDataExplorerClient(options);
        var actual = await sut.ExecuteQueryAsync(null);
        Assert.NotNull(actual);
    }
    
    [Fact]
    public async Task ExecuteQueryAsync_WhenErrorThrown_ResultShouldContainErrorMessage()
    {
        var sut = new AzureDataExplorerClient(options);
        var actual = await sut.ExecuteQueryAsync(null);
        Assert.False(string.IsNullOrEmpty(actual?.ErrorMessage));
    }
    
    [Fact]
    public async Task ExecuteQueryAsync_WhenErrorThrown_ResultSuccessShouldBeFalse()
    {
        var sut = new AzureDataExplorerClient(options);
        var actual = await sut.ExecuteQueryAsync(null);
        Assert.False(actual?.Success);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ExecuteQueryAsync_WhenQueryIsEmptyOrNull_ResultShouldBeFalse(string query)
    {
        var sut = new AzureDataExplorerClient(options);
        var actual = await sut.ExecuteQueryAsync(query);
        Assert.False(actual.Success);
    }

    [Fact]
    public void ParseResponse_WhenWebApiResponseIsNull_ShouldThrowArgumentNullException()
    {
        var sut = new AzureDataExplorerClient(options);
        Assert.Throws<ArgumentNullException>(() => sut.ParseResponse(null));
    }
    
    [Fact]
    public void ParseResponse_WhenWebApiResponseIsUnSuccessfull_ShouldThrowException()
    {
        var sut = new AzureDataExplorerClient(options);
        Assert.Throws<Exception>(() => sut.ParseResponse(new WebApiResponse(null, false)));
    }
    
    [Fact]
    public void ParseResponse_WhenWebApiResponseDataIsEmpty_ShouldThrowException()
    {
        var sut = new AzureDataExplorerClient(options);
        Assert.Throws<Exception>(() => sut.ParseResponse(new WebApiResponse(null, true)));
    }
    
    [Fact]
    public void ParseResponse_WhenWebApiResponseIsSet_ShouldReturnData()
    {
        var sut = new AzureDataExplorerClient(options);
        var fixtureData = System.IO.File.ReadAllText("Fixtures/datacontract_full_response.json");

        var actual = sut.ParseResponse(new WebApiResponse(fixtureData, true));
        Assert.NotNull(actual);
    }
    
    [Theory]
    [InlineData("availabilityResults")]
    [InlineData("browserTimings")]
    [InlineData("customEvents")]
    [InlineData("customMetrics")]
    [InlineData("dependencies")]
    [InlineData("exceptions")]
    [InlineData("pageViews")]
    [InlineData("performanceCounters")]
    [InlineData("requests")]
    [InlineData("traces")]
    public void ParseResponse_WhenWebApiResponseIsSet_ShouldHaveTablePresent(string tableName)
    {
        var sut = new AzureDataExplorerClient(options);
        var fixtureData = System.IO.File.ReadAllText("Fixtures/datacontract_full_response.json");

        var actual = sut.ParseResponse(new WebApiResponse(fixtureData, true));
        Assert.NotNull(actual.Tables.FirstOrDefault(x => x.TableName == tableName));
    }
    
    [Theory]
    [InlineData("availabilityResults")]
    [InlineData("browserTimings")]
    [InlineData("customEvents")]
    [InlineData("customMetrics")]
    [InlineData("dependencies")]
    [InlineData("exceptions")]
    [InlineData("pageViews")]
    [InlineData("performanceCounters")]
    [InlineData("requests")]
    [InlineData("traces")]
    public void ParseResponse_WhenWebApiResponseIsSet_ShouldHaveColumnsForTablePresent(string tableName)
    {
        var sut = new AzureDataExplorerClient(options);
        var fixtureData = System.IO.File.ReadAllText("Fixtures/datacontract_full_response.json");

        var actual = sut.ParseResponse(new WebApiResponse(fixtureData, true));
        Assert.True(actual?.Tables?.FirstOrDefault(x => x.TableName == tableName)?.Columns?.Count() > 0);
    }
}