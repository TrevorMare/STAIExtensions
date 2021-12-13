using System.Linq;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_Requests : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.Request>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_Requests.json";
    protected override string TableName => "requests";
    
}