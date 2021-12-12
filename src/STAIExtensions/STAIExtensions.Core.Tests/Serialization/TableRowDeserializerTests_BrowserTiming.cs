using System.Linq;
using STAIExtensions.Abstractions.WebApi;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_BrowserTiming
{

    [Fact]
    public void DeserializeTableRows_WhenDataSupplied_ShouldDeserializeRows()
    {
        var sut = new TableRowDeserializer();
        
        var apiClient = new AIQueryApiClient("123");
        var fixtureData = System.IO.File.ReadAllText("Fixtures/datacontract_full_response.json");
        var apiParseResponse = apiClient.ParseResponse(new WebApiResponse(fixtureData, true));

        var table = apiParseResponse.Tables.FirstOrDefault(x => x.TableName == "browserTimings");

        sut.DeserializeTableRows<Abstractions.DataContracts.Models.BrowserTiming>(table);
        


    }
    
    
    
}