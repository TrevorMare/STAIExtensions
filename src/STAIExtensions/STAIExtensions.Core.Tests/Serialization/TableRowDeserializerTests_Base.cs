using System.Linq;
using STAIExtensions.Abstractions.ApiClient.Models;
using STAIExtensions.Abstractions.WebApi;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public abstract class TableRowDeserializerTests_Base<T>
{
    protected abstract string FixtureFilePath { get; }
    protected abstract string TableName { get; }

    protected ApiClientQueryResultTable? GetApiResponse()
    {
        var apiClient = new AIQueryApiClient("123");
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