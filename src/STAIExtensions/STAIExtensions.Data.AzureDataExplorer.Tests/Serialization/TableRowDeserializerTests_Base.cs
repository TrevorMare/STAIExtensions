using System.Linq;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Data.AzureDataExplorer.Models;
using STAIExtensions.Data.AzureDataExplorer.Serialization;
using Xunit;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public abstract class TableRowDeserializerTests_Base<T> where T : IDataContract
{
    protected abstract string FixtureFilePath { get; }
    protected abstract string TableName { get; }

    internal Data.AzureDataExplorer.Models.ApiClientQueryResultTable? GetApiResponse()
    {
        var apiClient = new AzureDataExplorerClient(new TelemetryLoaderOptions("abc", "abc"));
        var fixtureData = System.IO.File.ReadAllText(FixtureFilePath);
        var apiParseResponse = apiClient.ParseResponse(new WebApiResponse(fixtureData, true));
        var table = apiParseResponse.Tables.FirstOrDefault(x => x.TableName == TableName);
        return table;
    }
    
    [Fact]
    public virtual void DeserializeTableRows_WhenDataSupplied_ShouldDeserializeObject()
    {
        var sut = new TableRowDeserializer();
        var parameters = GetApiResponse();

        var actual = sut.DeserializeTableRows<T>(parameters);
        Assert.NotNull(actual);
    }
    
    [Fact]
    public virtual void DeserializeTableRows_WhenDataSupplied_ShouldDeserializeRows()
    {
        var sut = new TableRowDeserializer();
        var parameters = GetApiResponse();

        var actual = sut.DeserializeTableRows<T>(parameters);
        Assert.Equal(2, actual.Count() );
    }
    
}