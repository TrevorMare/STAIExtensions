using System.Linq;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_BrowserTiming : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.BrowserTiming>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_BrowserTimings.json";
    protected override string TableName => "browserTimings";
    
    
    
}